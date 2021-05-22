using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudWeb.Services
{
    public class ContentService : BaseDao<ContentDto>, IContentService
    {
        public ResponseResult<bool> AddContent(ContentDto contentDto)
        {
            if (contentDto == null)
                return new ResponseResult<bool>(201, "请输入内容信息");

            const string sql = @"INSERT INTO [Ori_CloudWeb].[dbo].[Content]
            ([CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate] ,[IsPublic]  ,[IsTop],[IsDefault] ,[IsDel])
     VALUES
           (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColumnId,@Title,@Content,@ImgUrl1,@ImgUrl2,@LinkUrl,@Hits,@CreateDate ,@IsPublic  ,@IsTop,@IsDefault ,@IsDel)";
            contentDto.CreateTime = DateTime.Now;
            contentDto.ModifyTime = DateTime.Now;
            return new ResponseResult<bool>(Add(sql, contentDto));
        }

        public ResponseResult<bool> DeleteContent(dynamic[] ids)
        {
            if (ids.Length == 0)
                return new ResponseResult<bool>(201, "请输入内容id");

            string sql = "UPDATE FROM [Ori_CloudWeb].[dbo].[Content] SET [IsDel]=1 WHERE [ID]=@ids ";
            if (ids.Length > 1)
                sql = "UPDATE FROM [Ori_CloudWeb].[dbo].[Content] SET [IsDel]=1 WHERE [ID] in(@ids) ";

            return new ResponseResult<bool>(Delete(sql, new { ids = ids }));
        }

        public ResponseResult<bool> EditContent(ContentDto contentDto)
        {
            if (contentDto == null)
                return new ResponseResult<bool>(201, "请输入内容信息");

            const string sql = @"UPDATE [Ori_CloudWeb].[dbo].[Content]
   SET
      [ModifyTime] = @ModifyTime
      ,[Modifier] = @Modifier
      ,[ColumnId] = @ColumnId
      ,[Title] = @Title
      ,[Content] = @Content
      ,[ImgUrl1] = @ImgUrl1
      ,[ImgUrl2] = @ImgUrl2
      ,[LinkUrl] = @LinkUrl
      ,[Hits] = @Hits
      ,[CreateDate] = @CreateDate
      ,[IsPublic] = @IsPublic
      ,[IsTop] = @IsTop,
      ,[IsDefault] = @IsDefault
      ,[IsDel] = @IsDel
 WHERE [ID]=@Id";
            contentDto.ModifyTime = DateTime.Now;
            return new ResponseResult<bool>(Update(sql, contentDto));
        }

        public ResponseResult<IEnumerable<ContentDto>> GetAll(BaseParam para)
        {
            string sql = @"SELECT w2.n, w1.* FROM Content w1,(
            SELECT TOP (@page*@limit) row_number() OVER(ORDER BY Createtime DESC) n, Id FROM Content) w2
            WHERE w1.Id = w2.Id AND w2.n > (@limit*(@page-1)) ORDER BY w2.n ASC";
            string queryCountSql = "SELECT COUNT(*) FROM [Ori_CloudWeb].[dbo].[Content]";
            return new ResponseResult<IEnumerable<ContentDto>>(GetAll(sql, para), Count(queryCountSql));
        }

        public ResponseResult<ContentDto> GetContent(int id)
        {
            const string sql = "SELECT [Id],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate],[IsPublic],[IsTop],[IsDefault],[IsDel] FROM[Ori_CloudWeb].[dbo].[Content] WHERE [IsDel]=0 AND [Id]=@id";

            return new ResponseResult<ContentDto>(Find(sql, new { id = id }));
        }
    }
}
