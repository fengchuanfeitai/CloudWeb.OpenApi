using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using System.Collections.Generic;

namespace CloudWeb.Services
{
    public class ColumnService : BaseDao<ColumnDto>, IColumnService
    {

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <param name="columnDto"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddColumn(ColumnDto columnDto)
        {
            if (columnDto == null)
                return new ResponseResult<bool>(false,"请填栏目信息");

            const string sql = @"INSERT INTO Columns(CreateTime,ModifyTime ,Creator,Modifier ,ColName,Level,Summary,LocationUrl,Cover,ImgDesc1,ImgDesc2,Icon,Video,ParentId,Sort,IsNews,IsShow,IsDel)
            VALUES (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColName,@Level,@Summary,@LocationUrl,@Cover,@ImgDesc1,@ImgDesc2,@Icon ,@Video,@ParentId,@Sort,@IsNews,@IsShow,@IsDel)";
            return new ResponseResult<bool>(Add(sql, columnDto));
        }

        public ResponseResult<bool> DeleteColumn(dynamic[] ids)
        {
            if(ids.Length==0)
                return new ResponseResult<bool>(false, "请选择栏目id");

            string sql = "DELETE FROM [Ori_CloudWeb].[dbo].[Columns] WHERE  [ColumnId]=@ids ";
            if (ids.Length > 1)
                sql = "DELETE FROM [Ori_CloudWeb].[dbo].[Columns] WHERE  [ColumnId] in(@ids) ";

            return new ResponseResult<bool>(Delete(sql));
        }

        public ResponseResult<bool> EdittColumn(ColumnDto columnDto)
        {
            if (columnDto == null)
                return new ResponseResult<bool>(false, "请填栏目信息");

            string sql = @"
                UPDATE [Ori_CloudWeb].[dbo].[Columns]
                SET [CreateTime] = @CreateTime
                    ,[ModifyTime] = @ModifyTime
                    ,[Creator] = @Creator
                    ,[Modifier] = @Modifier
                    ,[ColName] = @ColName
                    ,[Level] =@Level
                    ,[Summary] =@Summary
                    ,[LocationUrl] = @LocationUrl
                    ,[Cover] =@Cover
                    ,[ImgDesc1] = @ImgDesc1
                    ,[ImgDesc2] = @ImgDesc2
                    ,[Icon] = @Icon
                    ,[Video] =@Video
                    ,[ParentId] =@ParentId
                    ,[Sort] =@Sort
                    ,[IsNews] =@IsNews
                    ,[IsShow] = @IsShow
                    ,[IsDel] = @IsDel
                WHERE [ColumnId]=@id";
            return new ResponseResult<bool>(Update(sql));
        }

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ColumnDto>> GetAll()
        {
            const string sql = @"SELECT [ColumnId],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColName],[Level],[Summary],[LocationUrl],[Cover],[ImgDesc1],[ImgDesc2],[Icon],[Video],[ParentId],[Sort],[IsNews] ,[IsShow] ,[IsDel]FROM[Ori_CloudWeb].[dbo].[Columns] WHERE [IsDel]=1  ORDER BY [CreateTime] DESC ";
            return new ResponseResult<IEnumerable<ColumnDto>>(GetAll(sql));
        }

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        public ResponseResult<ColumnDto> GetColumn(int id)
        {
            const string sql = @"SELECT [ColumnId],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColName],[Level],[Summary],[LocationUrl],[Cover],[ImgDesc1],[ImgDesc2],[Icon],[Video],[ParentId],[Sort],[IsNews] ,[IsShow] ,[IsDel]FROM[Ori_CloudWeb].[dbo].[Columns] WHERE WHERE [IsDel]=1  AND  [ColumnId]=@id";
            return new ResponseResult<ColumnDto>(Find(sql));
        }
    }
}
