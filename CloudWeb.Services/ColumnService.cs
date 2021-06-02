using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using System;
using System.Collections.Generic;

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

        #endregion

        #region 管理系统后台接口逻辑

        /// <summary>
        /// 添加栏目sql
        /// </summary>
        private const string Insert_Column_Sql = @"INSERT INTO Columns(CreateTime,ModifyTime ,Creator,Modifier ,ColName,Level,Summary,LocationUrl,CoverUrl,CoverLinks,Icon,Video,ParentId,Sort,IsNews,IsShow,IsDel)
            VALUES (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColName,@Level,@Summary,@LocationUrl,@CoverUrl,@CoverLinks,@Icon ,@Video,@ParentId,@Sort,@IsNews,@IsShow,@IsDel)";

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
            string colNameSql = "select count(1) from  Columns where  IsDel=0 and ColName=@ColName;";
            int count = Count(colNameSql, new { ColName = column.ColName });

            if (count >= 1)
                return result.SetFailMessage("栏目名称不能重复");

            //如果父级为0 ，则是第一级
            if (column.ParentId == 0)
                column.Level = 1;
            else
            {
                string sql = "select level from Columns where isdel=0 and ColumnId=@id";
                column.Level = Count(sql, new { id = column.ParentId }) + 1;//父级不为0，则查询父级level+1
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
        /// 删除：包括单条数据删除，多条数据删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DeleteColumn(int[] ids)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (ids.Length == 0)
                return result.SetFailMessage("请选择栏目id");
            string idStr = Util.ConverterUtil.StringSplit(ids);

            string sql1 = $"select count(1) from Columns where ParentId in ({idStr});";
            string condition = "";
            if (Count(sql1) > 0)
            {
                condition = $"UPDATE[Ori_CloudWeb].[dbo].[Columns] SET[IsDel] = 1 WHERE [ParentId] in ({ idStr}); ";
            }
            //业务逻辑：删除栏目的同时，删除对应栏目下的内容

            string sql = $"{condition}UPDATE [Ori_CloudWeb].[dbo].[Columns] SET[IsDel] = 1 WHERE  [ColumnId] in ({idStr});UPDATE Content set IsDel=1 where ColumnId in({idStr});";
            return result.SetData(Delete(sql));
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

            string getSql = "select * from Columns where  IsDel=0 and columnid=@id";
            ColumnDto column = Find<ColumnDto>(getSql, new { id = columnDto.ColumnId });

            if (column != null)
            {
                if (column.ColName != columnDto.ColName)
                {
                    //判断栏目名称是否唯一
                    string colNameSql = "select count(1) from  Columns where  IsDel=0 and ColName=@ColName;";
                    int count = Count(colNameSql, new { ColName = columnDto.ColName });

                    if (count > 1)
                        return result.SetFailMessage("栏目名称不能重复");
                }
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
            const string sql = @"SELECT ColumnId,ColName,Level,Summary,LocationUrl,CoverUrl,CoverLinks,Icon,Video,ParentId,Sort,IsNews,IsShow FROM[Ori_CloudWeb].[dbo].[Columns] WHERE  [IsDel]=0  AND  [ColumnId]=@id";
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
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDropDownDto>> GetDropDownList(int id)
        {
            string condition = "";
            if (id > 0)
                condition = " where ColumnId=@id";

            string sql = $"with columnsInfo as(select columnid, colname, ParentID, Level, IsNews,right('00' + cast(Sort as varchar(max)), 3) as Sort from columns where ParentID = 0 and IsDel = 0   union all select dt.columnid,dt.colname,dt.ParentID,dt.Level,dt.IsNews, c.Sort + '-' + right('00' + cast(dt.Sort as varchar(max)), 3) as Sort from columnsInfo as c join columns as dt on dt.ParentID = c.columnid)select columnid,colname,ParentID,IsNews,Level from columnsInfo {condition} order by Sort, Level; ";

            return new ResponseResult<IEnumerable<ColumnDropDownDto>>(GetAll<ColumnDropDownDto>(sql, new { id = id }));
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
