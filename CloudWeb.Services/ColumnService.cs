﻿using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
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
        #region 管理系统后台接口逻辑

        /// <summary>
        /// 添加栏目sql
        /// </summary>
        private const string Insert_Column_Sql = @"INSERT INTO Columns(CreateTime,ModifyTime ,Creator,Modifier ,ColName,Level,Summary,LocationUrl,CoverUrl,ImgDesc1,ImgDesc2,Icon,Video,ParentId,Sort,IsNews,IsShow,IsDel)
            VALUES (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColName,@Level,@Summary,@LocationUrl,@CoverUrl,@ImgDesc1,@ImgDesc2,@Icon ,@Video,@ParentId,@Sort,@IsNews,@IsShow,@IsDel)";

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

            if (column.ColName != columnDto.ColName)
            {
                //判断栏目名称是否唯一
                string colNameSql = "select count(1) from  Columns where  IsDel=0 and ColName=@ColName;";
                int count = Count(colNameSql, new { ColName = columnDto.ColName });

                if (count > 1)
                    return result.SetFailMessage("栏目名称不能重复");
            }

            string sql = @"
                UPDATE [Ori_CloudWeb].[dbo].[Columns]
                SET [ModifyTime] = @ModifyTime
                    ,[Creator] = @Creator
                    ,[Modifier] = @Modifier
                    ,[ColName] = @ColName
                    ,[Level] =@Level
                    ,[Summary] =@Summary
                    ,[LocationUrl] = @LocationUrl
                    ,[CoverUrl] =@CoverUrl
                    ,[ImgDesc1] = @ImgDesc1
                    ,[ImgDesc2] = @ImgDesc2
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
            //string sql = @"SELECT w2.num, w1.* FROM Columns w1,
            //(SELECT TOP (@PageIndex*@PageSize) row_number() OVER(ORDER BY Createtime DESC) num, ColumnId  FROM Columns where IsDel=0) w2
            //WHERE w1.ColumnId = w2.ColumnId AND w2.num > (@PageSize*(@PageIndex-1))  ORDER BY w2.num ASC";
            //string queryCountSql = "SELECT COUNT(*) FROM [Ori_CloudWeb].[dbo].[Columns] WHERE IsDel=0";

            //return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, pageParam), Count(queryCountSql));
            string sql = @"  SELECT * FROM Columns  where IsDel=0";
            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, pageParam));
        }

        /// <summary>
        /// 根据id查询数据
        /// </summary>
        /// <returns></returns>
        public ResponseResult<ColumnDto> GetColumn(int id)
        {
            const string sql = @"SELECT [ColumnId],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColName],[Level],[Summary],[LocationUrl],[CoverUrl],[ImgDesc1],[ImgDesc2],[Icon],[Video],[ParentId],[Sort],[IsNews] ,[IsShow] ,[IsDel]FROM[Ori_CloudWeb].[dbo].[Columns] WHERE  [IsDel]=0  AND  [ColumnId]=@id";
            return new ResponseResult<ColumnDto>(Find<ColumnDto>(sql, new { id = id }));
        }

        /// <summary>
        /// 根据父Id获取子级栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParent(int parentId)
        {
            const string sql = @"SELECT ColumnId,ColName FROM dbo.[Columns]
                  WHERE IsDel=0 AND IsShow=1 AND ParentId=@parentId";
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
                condition = " and ColumnId=@id";
            string sql = $"select ColumnId,ColName,Level from  Columns where  IsDel=0 and IsShow=1 {condition}; ";

            return new ResponseResult<IEnumerable<ColumnDropDownDto>>(GetAll<ColumnDropDownDto>(sql, new { id = id }));
        }


        #endregion

        #region 网站接口

        public ResponseResult<IEnumerable<ColumnDto>> GetIcons(int id)
        {
            string sql = "select * from Columns where isdel=0 and parentid=@id and level=2 order by sort asc";

            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql, new { id = id }));
        }

        public ResponseResult<IEnumerable<ColumnDto>> GetMenus()
        {
            string sql = "select * from Columns where isdel=0 and level=1 order by sort asc";
            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll<ColumnDto>(sql));
        }

        #endregion
    }
}
