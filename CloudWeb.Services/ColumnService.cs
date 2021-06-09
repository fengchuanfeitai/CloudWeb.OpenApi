using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CloudWeb.Services
{
    /// <summary>
    /// 栏目管理逻辑
    /// </summary>
    public class ColumnService : BaseDao, IColumnService
    {
        #region 私有方法

        private IList<CarouselDto> SplitCover(string cover, string coverLinks)
        {
            var resData = new List<CarouselDto>();

            if (cover == null || coverLinks == null)
                return resData;

            var coverArr = cover.Split(',');
            var coverLinkArr = coverLinks.Split(',');

            for (var i = 0; i < coverArr.Length; i++)
            {
                var data = new CarouselDto()
                {
                    CarouselUrl = coverArr[i],
                    CarouselLink = coverLinkArr[i]
                };
                resData.Add(data);
            }
            return resData;
        }

        private int GetSort(int? columnId)
        {
            var idStr = "";
            if (columnId != null)
                idStr = $"AND ColumnId != {columnId.Value}";
            var MaxSortsql = $"SELECT ISNULL(MAX(Sort),0) FROM dbo.[Columns] WHERE 1=1 {idStr}";
            return MaxSort(MaxSortsql) + 1;
        }

        #endregion

        #region 管理系统后台接口逻辑

        /// <summary>
        /// 添加栏目sql
        /// </summary>
        private const string Insert_Column_Sql = @"INSERT INTO Columns(CreateTime,ModifyTime ,Creator,Modifier ,ColName,Level,Summary,LocationUrl,CoverUrl,CoverLinks,Icon,Video,ParentId,Sort,IsNews,Module,IsShow,IsDel)
            VALUES (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColName,@Level,@Summary,@LocationUrl,@CoverUrl,@CoverLinks,@Icon ,@Video,@ParentId,@Sort,@IsNews,@Module,@IsShow,@IsDel)";

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddColumn(ColumnDto column)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (column == null)
                return result.SetFailMessage("请填栏目信息");

            //判断栏目名称是否唯一
            string colNameSql = "select count(1) from  Columns where  IsDel=0 and ColName=@ColName and ParentId=@ParentId;";
            int count = Count(colNameSql, new { ColName = column.ColName, ParentId = column.ParentId });

            if (count > 0)
                return result.SetFailMessage("同一级别栏目，栏目名称不能重复");

            if (column.Sort == null)
                column.Sort = GetSort(null);


            //如果父级为0 ，则是第一级
            //if (column.ParentId == 0)
            //    column.Level = 1;
            //else
            //{
            //    string sql = "select level from Columns where isdel=0 and ColumnId=@id";
            //    column.Level = Count(sql, new { id = column.ParentId }) + 1;//父级不为0，则查询父级level+1
            //}
            if (column.Level < 3)
            {
                column.IsNews = false;
                column.Module = 0;
            }
            column.CreateTime = DateTime.Now;
            column.ModifyTime = DateTime.Now;
            return result.SetData(Add(Insert_Column_Sql, column));
        }

        /// <summary>
        /// 改变显示状态
        /// </summary>
        /// <param name="showStatusParam">状态参数</param>
        /// <returns></returns>
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE Columns SET IsShow = @ShowStatus WHERE ColumnId = @Id";
            bool isSuccess = Update(sql, showStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }

        /// <summary>
        /// 栏目下级是否包含数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> IsContainsContent(int[] ids)
        {
            //判读当前栏目下是否存在内容，有则提示"当前栏目中包含内容数据，是否同时删除？"
            string idStr = Util.ConverterUtil.StringSplit(ids);
            string contentSql = $"select COUNT(1) from Content where IsDel=0 and columnid in  ({idStr});";
            ResponseResult<bool> result = new ResponseResult<bool>();

            if (Count(contentSql) > 0)
                result.SetFailMessage(ContantMsg.DelColumn_ContainsContent_Msg);
            else
                result.SetData(true);
            return result;
        }

        /// <summary>
        /// 删除：包括单条数据删除，多条数据删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DeleteColumn(int[] ids)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (ids.Length == 0)
                return result.SetFailMessage("请选择栏目");
            //判断是否包含预设栏目，如果存在，则提示“当前选项包含预设栏目，不允许删除”，栏目来源数据表中，配置再配置文件中
            //获取配置文件中的数据
            //添加 json 文件路径
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();
            //appsetting.json中获取图片的配置
            string SystemcolumnsId = configurationRoot.GetSection("SystemColumnIds:ParentColumnId").Value;

            foreach (int id in ids)
            {
                if (SystemcolumnsId.Contains(id.ToString()))
                    return new ResponseResult<bool>($"当前选中栏目中包含预设数据编号{SystemcolumnsId},请重新选择");
            }

            string idStr = Util.ConverterUtil.StringSplit(ids);
            string sql1 = $"select count(1) from Columns where ParentId in ({idStr});";
            string condition = "";
            if (Count(sql1) > 0)
            {
                condition = $"UPDATE[Ori_CloudWeb].[dbo].[Columns] SET[IsDel] = 1 WHERE [ParentId] in ({ idStr}); ";
            }
            //业务逻辑：删除栏目的同时，删除对应栏目下的内容

            string sql = $"{condition}UPDATE [Ori_CloudWeb].[dbo].[Columns] SET[IsDel] = 1 WHERE  [ColumnId] in ({idStr});UPDATE Content set IsDel=1 where ColumnId in({idStr});";
            bool isSuccess = Delete(sql);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, ContantMsg.DelColumn_Ok_Msg);
            else
                result.Set((int)HttpStatusCode.fail, ContantMsg.DelColumn_Fail_Msg);
            return result;
        }

        /// <summary>
        /// 编辑当前栏目
        /// </summary>
        /// <param name="columnDto"></param>
        /// <returns></returns>
        public ResponseResult<bool> EditColumn(ColumnDto columnDto)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (columnDto == null)
                return result.SetFailMessage("请填写栏目信息");

            string getSql = "select ColName,ColumnId from Columns where  IsDel=0 and columnid=@id";
            string colName = Find<string>(getSql, new { id = columnDto.ColumnId });

            if (!string.IsNullOrEmpty(colName))
            {
                if (colName != columnDto.ColName)
                {
                    //判断栏目名称是否唯一
                    string colNameSql = "select count(1) from  Columns where  IsDel=0 and ColName=@ColName and ParentId=@ParentId;";
                    int count = Count(colNameSql, new { ColName = columnDto.ColName, ParentId = columnDto.ParentId });

                    if (count > 0)
                        return result.SetFailMessage("同一级别栏目，栏目名称不能重复");
                }
            }

            if (columnDto.ParentId == columnDto.ColumnId)
                return result.SetFailMessage("父级栏目，不能选择自己，请重新选择");

            if (columnDto.Sort == null)
                columnDto.Sort = GetSort(columnDto.ColumnId);
            //如果父级为0 ，则是第一级
            //if (columnDto.ParentId == 0)
            //    columnDto.Level = 1;
            //else
            //{
            //    string levelSql = "select level from Columns where isdel=0 and ColumnId=@id";
            //    columnDto.Level = Count(levelSql, new { id = columnDto.ParentId }) + 1;//父级不为0，则查询父级level+1
            //}
            if (columnDto.Level < 3)
            {
                columnDto.IsNews = false;
                columnDto.Module = 0;
            }

            string sql = @"
                UPDATE [Ori_CloudWeb].[dbo].[Columns]
                SET [ModifyTime] = @ModifyTime
                    ,[Modifier] = @Modifier
                    ,[ColName] = @ColName
                    ,[Level] =@Level
                    ,[Summary] =@Summary
                    ,[LocationUrl] = @LocationUrl
                    ,[CoverUrl] =@CoverUrl
                    ,[CoverLinks]=@CoverLinks
                    ,[Icon] = @Icon
                    ,[Video] =@Video
                    ,[ParentId] =@ParentId
                    ,[Sort] =@Sort
                    ,[IsNews] =@IsNews
                    ,[Module] =@Module
                    ,[IsShow] = @IsShow
                WHERE [ColumnId]=@ColumnId";
            columnDto.ModifyTime = DateTime.Now;
            return result.SetData(Update(sql, columnDto));
        }

        /// <summary>
        /// 分页查询所有栏目
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDto>> GetAll(BaseParam pageParam)
        {
            string sql = @"  SELECT * FROM Columns  where IsDel=0";
            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, pageParam));
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <returns></returns>
        public ResponseResult<ColumnSelectDto> GetColumn(int id)
        {
            const string sql = @"SELECT ColumnId,ColName,Level,Summary,LocationUrl,CoverUrl,CoverLinks,Icon,Video,ParentId,Sort,IsNews,IsShow,Module FROM[Ori_CloudWeb].[dbo].[Columns] WHERE  [IsDel]=0  AND  [ColumnId]=@id";
            return new ResponseResult<ColumnSelectDto>(Find<ColumnSelectDto>(sql, new { id = id }));
        }

        /// <summary>
        /// 根据父Id获取子级栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParent(int parentId, int? level)
        {
            var levelStr = "";
            if (level != null)
                levelStr = $" AND Level ={level}";

            string sql = $"SELECT ColumnId,ColName FROM dbo.[Columns] WHERE IsDel=0 AND IsShow=1 AND ParentId=@parentId {levelStr}";

            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, new { parentId = parentId }));
        }

        /// <summary>
        /// 下拉框数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="existTopLevel">是否存在顶级</param>
        /// <returns></returns>
        public ResponseResult<IList<ColumnDropDownDto>> GetDropDownList(int? id, bool existTopLevel)
        {
            var list = new List<ColumnDropDownDto>();
            string condition = "";
            if (id > 0)
                condition = " where ColumnId=@id";

            if (id != 0)
            {
                string sql = $"with columnsInfo as(select columnid, colname, ParentID, Level, IsDel, IsNews,right('00' + cast(Sort as varchar(max)), 3) as Sort from columns where ParentID = 0 and IsDel = 0   union all select  dt.columnid,dt.colname,dt.ParentID,dt.Level,dt.IsDel,dt.IsNews, c.Sort + '-' + right('00' + cast(dt.Sort as varchar(max)), 3) as Sort from columnsInfo as c join columns as dt on dt.ParentID = c.columnid where dt.IsDel=0 and c.IsDel=0)select columnid,colname,ParentID,IsNews,Level,IsDel from columnsInfo {condition} order by Sort, Level; ";

                var downEnumerable = GetAll<ColumnDropDownDto>(sql, new { id = id });
                list = downEnumerable.ToList();
            }
            if (existTopLevel)
            {
                var first = new ColumnDropDownDto()
                {
                    ColumnId = 0,
                    ColName = "顶级",
                    Level = 0,
                    IsNews = 0
                };
                list.Insert(0, first);
            }
            return new ResponseResult<IList<ColumnDropDownDto>>(list);
        }


        public ResponseResult<IList<SelectListItem>> GetModuleDownList()
        {
            var list = EnumUtil.GetSelectListItem<ModuleType>();
            return new ResponseResult<IList<SelectListItem>>(list);
        }

        #endregion

        #region 网站接口

        public ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParentId(int parentId)
        {
            string sql = "SELECT * FROM dbo.[Columns] WHERE IsDel = 0 AND IsShow = 1 AND ParentId=@parentId";
            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, new { parentId = parentId }));
        }

        public ResponseResult<IList<CarouselDto>> GetCarouselImg(int columnId)
        {
            var result = new ResponseResult<IList<CarouselDto>>();
            string sql = "SELECT CoverUrl,CoverLinks FROM dbo.[Columns] WHERE IsDel=0 AND IsShow=1 AND  ColumnId=@columnId";
            var column = Find<ColumnDto>(sql, new { columnId = columnId });
            if (column == null)
                return result.SetFailMessage("获取轮播图失败，栏目编号不存在");

            var Carousel = SplitCover(column.CoverUrl, column.CoverLinks);

            return result.SetData(Carousel);
        }

        public ResponseResult<IEnumerable<ColumnDto>> GetExperimentCol(int columnId, int level)
        {
            string sql = "SELECT * FROM dbo.[Columns] WHERE IsDel=0 AND IsShow=1 AND ParentId = @ColumnId AND [Level]=@Level ORDER BY Sort ASC ";

            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, new { ColumnId = columnId, Level = level }));
        }
        #endregion
    }
}
