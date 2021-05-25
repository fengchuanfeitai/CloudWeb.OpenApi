using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.Util;
using System;
using System.Collections.Generic;

namespace CloudWeb.Services
{
    public class ContentService : BaseDao, IContentService
    {
        /// <summary>
        /// 插入sql
        /// </summary>
        private const string Insert_Content_Sql = @"INSERT INTO [Ori_CloudWeb].[dbo].[Content]
            ([CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate] ,[IsPublic]  ,[IsCarousel],[IsDefault] ,[IsDel],[Sort])
          VALUES(@CreateTime,@ModifyTime,@Creator,@Modifier,@ColumnId,@Title,@Content,@ImgUrl1,@ImgUrl2,@LinkUrl,@Hits,@CreateDate ,@IsPublic ,@IsCarousel,@IsDefault ,@IsDel,@Sort)";

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="contentDto"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddContent(ContentParam content)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (content == null)
                return result.SetFailMessage("请输入内容信息");

            //默认值
            content.CreateTime = DateTime.Now;
            content.ModifyTime = DateTime.Now;
            content.CreateDate = DateTime.Now.ToString();
            result.SetData(Add(Insert_Content_Sql, content));
            return result;
        }


        /// <summary>
        /// 改变添加轮播
        /// </summary>
        /// <param name="TopStatusParam">状态参数</param>
        /// <returns></returns>
        public ResponseResult ChangeDefaultStatus(DefaultStatusParam defaultStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE content SET IsDefault = @DefaultStatus WHERE Id = @Id";
            bool isSuccess = Update(sql, defaultStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }

        /// <summary>
        /// 改变发布状态
        /// </summary>
        /// <param name="PublicStatusParam">状态参数</param>
        /// <returns></returns>
        public ResponseResult ChangePublicStatus(PublicStatusParam publicStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE content SET IsPublic = @PublicStatus WHERE Id = @Id";
            bool isSuccess = Update(sql, publicStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }

        /// <summary>
        /// 改变添加首页
        /// </summary>
        /// <param name="DefaultStatusParam">状态参数</param>
        /// <returns></returns>

        public ResponseResult ChangeTopStatus(TopStatusParam topStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE content SET IsCarousel = @TopStatus WHERE Id = @Id";
            bool isSuccess = Update(sql, topStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }

        /// <summary>
        /// 删除内容,全选删除、单条删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DeleteContent(int[] ids)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (ids.Length == 0)
                return result.SetFailMessage("请选择要删除的内容");

            string idStr = ConverterUtil.StringSplit(ids);
            string sql = $"UPDATE  [Ori_CloudWeb].[dbo].[Content] SET [IsDel]=1 WHERE [ID] in({idStr}) ";
            return result.SetData(Delete(sql, new { ids = ids }));
        }

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="contentDto"></param>
        /// <returns></returns>
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
                      ,[IsCarousel] = @IsCarousel
                      ,[IsDefault] = @IsDefault
                      ,[IsDel] = @IsDel
                      ,[Sort] = @Sort
                 WHERE [ID]=@Id";
            contentDto.ModifyTime = DateTime.Now;
            return new ResponseResult<bool>(Update(sql, contentDto));
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ContentDto>> GetAll(BaseParam para)
        {
            string sql = @"SELECT w2.n, w1.* FROM Content w1,(
            SELECT TOP (@PageIndex*@PageSize) row_number() OVER(ORDER BY Createtime DESC) n, Id FROM Content where isdel=0) w2
            WHERE w1.Id = w2.Id AND w2.n > (@PageSize*(@PageIndex-1)) ORDER BY w2.n ASC";
            string queryCountSql = "SELECT COUNT(*) FROM [Ori_CloudWeb].[dbo].[Content] where isdel=0 ";
            return new ResponseResult<IEnumerable<ContentDto>>(GetAll<ContentDto>(sql, para), Count(queryCountSql));
        }

        /// <summary>
        /// 根据id查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<ContentDto> GetContent(int id)
        {
            const string sql = "SELECT [Id],[CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate],[IsPublic],[IsCarousel],[IsDefault],[IsDel],[Sort] FROM[Ori_CloudWeb].[dbo].[Content] WHERE [IsDel]=0 AND [Id]=@id";

            return new ResponseResult<ContentDto>(Find<ContentDto>(sql, new { id = id }));
        }
    }
}
