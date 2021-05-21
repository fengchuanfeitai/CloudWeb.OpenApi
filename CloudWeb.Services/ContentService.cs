using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
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
                return new ResponseResult<bool>(false, "请输入内容信息");

            const string sql = @"INSERT INTO [Ori_CloudWeb].[dbo].[Content]
            ([CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate] ,[IsPublic]  ,[IsTop],[IsDefault] ,[IsDel])
     VALUES
           (@CreateTime,@ModifyTime,@Creator,@Modifier,@ColumnId,@Title,@Content,@ImgUrl1,@ImgUrl2,@LinkUrl,@Hits,@CreateDate ,@IsPublic  ,@IsTop,@IsDefault ,@IsDel)";

            return new ResponseResult<bool>(Add(sql, contentDto));
        }

        public ResponseResult<bool> DeleteContent(dynamic[] ids)
        {
            if (ids.Length == 0)
                return new ResponseResult<bool>(false, "请输入内容id");

            string sql = "DELETE FROM [Ori_CloudWeb].[dbo].[Content] WHERE [ID]=@ids ";
            if (ids.Length > 1)
                sql = "DELETE FROM [Ori_CloudWeb].[dbo].[Content] WHERE [ID] in(@ids) ";

            return new ResponseResult<bool>(Delete(sql, ids));
        }

        public ResponseResult<bool> EdittContent(ContentDto contentDto)
        {
            if (contentDto == null)
                return new ResponseResult<bool>(false, "请输入内容信息");

            const string sql = "";

            return new ResponseResult<bool>(Update(sql, contentDto));
        }

        public ResponseResult<IEnumerable<ContentDto>> GetAll()
        {
            const string sql = "SELECT [Id],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate],[IsPublic],[IsTop],[IsDefault],[IsDel] FROM[Ori_CloudWeb].[dbo].[Content] WHERE [IsDel]=0 ORDER BY [CREATETIME] DESC";

            return new ResponseResult<IEnumerable<ContentDto>>(GetAll(sql));
        }

        public ResponseResult<ContentDto> GetContent(int id)
        {
            const string sql = "SELECT [Id],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate],[IsPublic],[IsTop],[IsDefault],[IsDel] FROM[Ori_CloudWeb].[dbo].[Content] WHERE [IsDel]=0 AND [Id]=@id";

            return new ResponseResult<ContentDto>(Find(sql, id));
        }
    }
}
