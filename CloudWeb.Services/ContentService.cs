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
        #region 私有函数

        /// <summary>
        /// 插入sql
        /// </summary>
        private const string Insert_Content_Sql = @"INSERT INTO [Ori_CloudWeb].[dbo].[Content]
            ([CreateTime],[ModifyTime],[Creator],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate] ,[IsPublic]  ,[IsCarousel],[IsDefault] ,[IsDel],[Sort])
          VALUES(@CreateTime,@ModifyTime,@Creator,@Modifier,@ColumnId,@Title,@Content,@ImgUrl1,@ImgUrl2,@LinkUrl,@Hits,@CreateDate ,@IsPublic ,@IsCarousel,@IsDefault ,@IsDel,@Sort)";

        private IEnumerable<ContentDto> DealWithContent(IEnumerable<ContentDto> contents,
            bool filterHtml, int? titleCut, int? contentCut)
        {
            if (!filterHtml && titleCut == null && contentCut == null)
                return contents;
            foreach (var item in contents)
            {
                if (filterHtml)
                {
                    if (!string.IsNullOrEmpty(item.Content))
                        item.Content = TextUtil.FilterHtml(item.Content);
                }
                if (titleCut != null)
                {
                    item.TruncatTitle = TextUtil.StringTruncat(item.Title, titleCut.Value, "");
                }
                if (contentCut != null)
                {
                    item.Content = TextUtil.StringTruncat(item.Content, contentCut.Value, "......");
                }
            }

            return contents;
        }

        #endregion

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


            //判断当前数据中栏目是否已经删除
            bool isDel = ColumnIsDelete(content.ColumnId);

            if (isDel)
            {
                return result.SetFailMessage(ContantMsg.EditContent_ColumnIsDel_Msg);
            }

            //默认值
            content.CreateTime = DateTime.Now;
            content.ModifyTime = DateTime.Now;
            content.CreateDate = DateTime.Now;
            content.Hits = 0;
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
                result.Set((int)HttpStatusCode.OK, ContantMsg.ChangeStatus_OK_Msg);
            else
                result.Set((int)HttpStatusCode.fail, ContantMsg.ChangeStatus_Fail_Msg);
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
                result.Set((int)HttpStatusCode.OK, ContantMsg.ChangeStatus_OK_Msg);
            else
                result.Set((int)HttpStatusCode.fail, ContantMsg.ChangeStatus_Fail_Msg);
            return result;
        }

        /// <summary>
        /// 改变添加首页
        /// </summary>
        /// <param name="DefaultStatusParam">状态参数</param>
        /// <returns></returns>

        public ResponseResult ChangeCarouselStatus(CarouselStatusParam carouselStatusParam)
        {
            ResponseResult result = new ResponseResult();
            string sql = "UPDATE content SET IsCarousel = @CarouselStatus WHERE Id = @Id";
            bool isSuccess = Update(sql, carouselStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, ContantMsg.ChangeStatus_OK_Msg);
            else
                result.Set((int)HttpStatusCode.fail, ContantMsg.ChangeStatus_Fail_Msg);
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
                return result.SetFailMessage(ContantMsg.DeleteContent_NoId_Msg);

            string idStr = ConverterUtil.StringSplit(ids);
            string sql = $"UPDATE  [Ori_CloudWeb].[dbo].[Content] SET [IsDel]=1 WHERE [ID] in({idStr}) ";
            return result.SetData(Delete(sql, new { ids = ids }));
        }

        /// <summary>
        /// 判断项目是否删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ColumnIsDelete(int id)
        {
            string sql = "select COUNT(1) from Columns where IsDel=1 and ColumnId=@id;";
            return Count(sql, new { id = id }) > 0;
        }

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="contentDto"></param>
        /// <returns></returns>
        public ResponseResult<bool> EditContent(ContentDto contentDto)
        {
            ResponseResult<bool> result = new ResponseResult<bool>();
            if (contentDto == null)
                return result.SetFailMessage(ContantMsg.EditContent_IsNull_Msg);

            //判断当前数据中栏目是否已经删除
            bool isDel = ColumnIsDelete(contentDto.ColumnId);

            if (isDel)
            {
                return result.SetFailMessage(ContantMsg.EditContent_ColumnIsDel_Msg);
            }

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
                      ,[Sort] = @Sort
                 WHERE [ID]=@Id";
            contentDto.ModifyTime = DateTime.Now;
            contentDto.Hits = 0;

            result.SetData(Update(sql, contentDto));
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<ContentDto>> GetAll(SearchParam para)
        {
            string columnCondition = "", titleCondition = "";
            if (para.ColumnId > 0)
                columnCondition = " AND ColumnId=@ColumnId";

            if (!string.IsNullOrEmpty(para.TitleKeyword))
                titleCondition = " AND title like '%" + para.TitleKeyword + "%'";

            string sql = $"SELECT w2.n, w3.* from (SELECT w1.*,c.ColName FROM Content w1 inner join Columns c on w1.ColumnId=c.ColumnId ) w3,( SELECT TOP  (@PageIndex*@PageSize) row_number() OVER(ORDER BY Createtime DESC) n, Id FROM Content where isdel=0  {columnCondition}{titleCondition}) w2  WHERE w3.Id = w2.Id AND w2.n > (@PageSize*(@PageIndex-1)) ORDER BY w2.n ASC;";
            string queryCountSql = $"SELECT COUNT(*) FROM [Ori_CloudWeb].[dbo].[Content] where isdel=0  {columnCondition}{titleCondition}";
            return new ResponseResult<IEnumerable<ContentDto>>(GetAll<ContentDto>(sql, para), Count(queryCountSql, para));
        }

        /// <summary>
        /// 根据id查询内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<ContentSelectDto> GetContent(int id)
        {
            const string sql = "SELECT [Id],[Modifier],[ColumnId],[Title],[Content],[ImgUrl1],[ImgUrl2],[LinkUrl],[Hits],[CreateDate],[IsPublic],[IsCarousel],[IsDefault],[IsDel],[Sort] FROM[Ori_CloudWeb].[dbo].[Content] WHERE [IsDel]=0 AND [Id]=@id";

            return new ResponseResult<ContentSelectDto>(Find<ContentSelectDto>(sql, new { id = id }));
        }

        #region 网站接口

        public ResponseResult<IEnumerable<ContentDto>> GetIndexNews(IndexNewsParam param)
        {
            var result = new ResponseResult<IEnumerable<ContentDto>>();

            int isCarouselInt = param.IsCarousel ? 1 : 0;

            string sql = $"SELECT * FROM dbo.[Content] WHERE IsDel=0 AND IsPublic=1 AND IsDefault=1 AND IsCarousel={isCarouselInt} AND ColumnId IN(SELECT ColumnId FROM dbo.[Columns] WHERE (ColumnId = 2 OR ParentId = 2) AND IsShow =1 AND IsDel =0) ORDER BY Sort ASC,CreateTime DESC";

            var news = GetAll<ContentDto>(sql);
            if (news == null)
                return result.SetData(news);

            var processedData = DealWithContent(news, param.FilterHtml, param.TitleCut, param.ContentCut);
            return result.SetData(processedData);
        }

        public ResponseResult<IEnumerable<ContentDto>> GetConByCol(ConByColParam param)
        {
            var result = new ResponseResult<IEnumerable<ContentDto>>();
            var IsCarouselStr = "";
            var IsDefaultStr = "";
            if (param.IsCarousel != null)
            {
                int IsCarouselInt = param.IsCarousel.Value ? 1 : 0;
                IsCarouselStr = $" AND IsCarousel = {IsCarouselInt}";
            }
            if (param.IsDefault != null)
            {
                int IsDefaultInt = param.IsDefault.Value ? 1 : 0;
                IsDefaultStr = $" AND IsDefault = {IsDefaultInt}";
            }

            string sql = $"SELECT * FROM dbo.Content WHERE IsDel = 0 AND IsPublic=1 AND ColumnId =@ColumnId {IsCarouselStr} {IsDefaultStr}";

            var contents = GetAll<ContentDto>(sql, param);
            if (contents == null)
                return result.SetData(contents);

            //处理数据
            var processedData = DealWithContent(contents, param.FilterHtml, param.TitleCut, param.ContentCut);
            return result.SetData(processedData);
        }

        public ResponseResult<IEnumerable<ContentDto>> GetColPageContent(ConSearchParam param)
        {
            var result = new ResponseResult<IEnumerable<ContentDto>>();

            var ColumnSearch = "";
            var KeywordSearch = "";
            if (param.ObjId != null)
                ColumnSearch = $" AND ColumnId={param.ObjId}";

            if (!string.IsNullOrEmpty(param.KeyWord))
                KeywordSearch = "AND (Title LIKE '%" + param.KeyWord + "%' OR Content LIKE '%" + param.KeyWord + "%')";

            string AllPaPersSql = $"SELECT c2.[Index],c1.* FROM dbo.Content c1,(SELECT TOP(@PageIndex*@PageSize) ROW_NUMBER() OVER(ORDER BY Sort ASC, CreateTime DESC) AS[Index], Id,ColumnId FROM dbo.Content WHERE IsDel = 0 AND IsPublic = 1 {ColumnSearch} {KeywordSearch} AND ColumnId IN (SELECT ColumnId FROM dbo.[Columns] WHERE ColumnId = {param.MasterId} OR ParentId = {param.MasterId} AND IsDel = 0 AND IsShow = 1)) c2  WHERE c1.Id = c2.Id AND c2.[Index] > ((@PageIndex -1)*@PageSize) ";

            var CountSql = $"SELECT COUNT(*) FROM dbo.Content WHERE IsPublic = 1 AND IsDel = 0 {ColumnSearch} {KeywordSearch} AND ColumnId IN (SELECT ColumnId FROM dbo.[Columns] WHERE ColumnId = {param.MasterId} OR ParentId = {param.MasterId} AND IsDel = 0 AND IsShow = 1)";

            var contents = GetAll<ContentDto>(AllPaPersSql, param);
            var count = Count(CountSql);
            if (contents == null)
                return result.SetData(contents, count);

            var processedData = DealWithContent(contents, param.FilterHtml, param.TitleCut, param.ContentCut);

            return result.SetData(contents, count);
        }

        #endregion

    }
}
