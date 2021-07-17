using System;
using System.Collections.Generic;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.Util;

namespace CloudWeb.Services
{
    public class CorpProductsService : BaseDao, ICorpProductsService
    {
        #region 私有方法

        public string GetCorpName(int corpId)
        {
            const string SelSql = @"SELECT [Name] FROM dbo.Corporations WHERE CorpId =@corpId";
            var Corporation = Find<CorporationDto>(SelSql, new { corpId = corpId });
            return Corporation.Name;
        }

        private int GetSort(int? id)
        {
            var idStr = "";
            if (id != null)
                idStr = $"AND id != {id.Value}";
            var MaxSortsql = $"SELECT ISNULL(MAX(Sort),0) FROM dbo.CorpProducts WHERE 1=1 {idStr}";
            return MaxSort(MaxSortsql) + 1;
        }

        #endregion

        #region 公用方法

        #endregion

        #region 后台逻辑接口

        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddProduct(CorpProductsDto corpProduct)
        {
            corpProduct.CreateTime = DateTime.Now;
            corpProduct.ModifyTime = corpProduct.CreateTime;
            corpProduct.Modifier = corpProduct.Creator;
            if (corpProduct.Sort == null)
                corpProduct.Sort = GetSort(null);

            const string InsertSql = @"INSERT INTO dbo.CorpProducts
                 (CreateTime,ModifyTime,Creator,Modifier,[Name],Cover,
                  Content,CorpId,LocationUrl,Sort,IsShow,IsDel)
                  VALUES 
                 (@CreateTime,@ModifyTime,@Creator,@Modifier,@Name,@Cover,
                  @Content,@CorpId,@LocationUrl,@Sort,@IsShow,@IsDel )";

            return new ResponseResult<bool>(Add(InsertSql, corpProduct));
        }

        /// <summary>
        /// 逻辑删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DelProduct(int[] ids)
        {
            var result = new ResponseResult<bool>();
            if (ids.Length == 0)
                return result.SetFailMessage("请选择产品");

            if (ids.Length == 1)
            {
                const string delSql = "UPDATE dbo.CorpProducts SET IsDel =1 WHERE Id =@ids";
                return result.SetData(Delete(delSql, new { ids = ids }));
            }
            else
            {
                string idsStr = ConverterUtil.StringSplit(ids);
                string delSql = $"UPDATE dbo.CorpProducts SET IsDel =1 WHERE Id In ({idsStr})";
                return result.SetData(Delete(delSql, new { ids = ids }));
            }
        }

        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateProduct(CorpProductsDto corpProduct)
        {
            var CorpProduct = GetProductsById(corpProduct.Id.Value);
            if (CorpProduct.code != 200)
                return new ResponseResult<bool>(201, CorpProduct.msg);
            if (CorpProduct.data == null)
                return new ResponseResult<bool>(201, "产品不存在。");

            if (Equals(corpProduct, CorpProduct))
                return new ResponseResult<bool>(200, "数据无更改");

            corpProduct.ModifyTime = DateTime.Now;
            corpProduct.Modifier = corpProduct.Creator;
            if (corpProduct.Sort == null)
                corpProduct.Sort = GetSort(corpProduct.Id.Value);

            const string UpdateSql = @"UPDATE dbo.CorpProducts SET 
                  ModifyTime=@ModifyTime,Modifier=@Modifier,[Name]=@Name,Cover=@Cover,Content=@Content,
                  CorpId=@CorpId,LocationUrl=@LocationUrl,Sort=@Sort,IsShow=@IsShow,IsDel=@IsDel 
                  WHERE IsDel=0 AND Id=@Id";

            return new ResponseResult<bool>(Update(UpdateSql, corpProduct));
        }

        /// <summary>
        /// 获取公司的产品
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        public ResponseResult<IEnumerable<CorpProductsDto>> GetProductsByCorpId(int corpId)
        {
            const string SelSql = @"SELECT Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],
                                   Cover,Content,CorpId,LocationUrl,Sort,IsShow,IsDel
                                    FROM dbo.CorpProducts WHERE IsDel=0 AND CorpId=@corpId
                                   ORDER BY Sort DESC, CreateTime DESC ";

            return new ResponseResult<IEnumerable<CorpProductsDto>>(GetAll<CorpProductsDto>(SelSql, new { corpId = corpId })); ;
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<CorpProductsDto> GetProductsById(int id)
        {
            const string SelSql = @"SELECT Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],
                                    Cover,Content,CorpId,LocationUrl,Sort,IsShow,IsDel
                                    FROM dbo.CorpProducts WHERE IsDel=0 AND Id=@id";

            return new ResponseResult<CorpProductsDto>(Find<CorpProductsDto>(SelSql, new { id = id }));
        }

        /// <summary>
        /// 获取分页产品信息
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageProductList(ProductSearchParam pageParam)
        {
            string SearchCorpId = "";
            string SearchName = "";

            if (pageParam.CorpId != null)
            {
                SearchCorpId = " AND CorpId= @CorpId";
            }

            if (!string.IsNullOrEmpty(pageParam.NameKeyword))
            {
                SearchName = " AND Name like '%" + pageParam.NameKeyword + "%'";
            }

            string SelSql = $"SELECT cp1.Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],Cover,Content,LocationUrl,CorpId,Sort,IsShow,IsDel FROM dbo.CorpProducts cp1,(SELECT TOP (@PageIndex * @Pagesize) ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS [Index],Id FROM CorpProducts where IsDel=0) cp2 WHERE cp1.Id = cp2.Id {SearchCorpId} {SearchName} AND cp2.[Index] > (@PageSize * (@PageIndex - 1)) ORDER BY cp2.[Index] asc";

            var List = GetAll<CorpProductsDto>(SelSql, pageParam);
            foreach (var item in List)
            {
                //获取公司名
                item.CorpName = GetCorpName(item.CorpId);
            }
            string CountSql = $"SELECT COUNT(*) FROM dbo.CorpProducts WHERE IsDel=0 {SearchCorpId} {SearchName}";
            return new ResponseResult<IEnumerable<CorpProductsDto>>(List, Count(CountSql));
        }

        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="showStatusParam"></param>
        /// <returns></returns>
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            ResponseResult result = new ResponseResult();

            string sql = "UPDATE dbo.CorpProducts SET IsShow = @ShowStatus WHERE Id = @Id";

            bool isSuccess = Update(sql, showStatusParam);

            if (isSuccess)
                result.Set((int)HttpStatusCode.OK, "修改状态成功");
            else
                result.Set((int)HttpStatusCode.fail, "修改状态失败");
            return result;
        }

        #endregion

        #region 网站接口

        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageProduct(ProductSearchParam param)
        {

            string sql = $"SELECT w2.num, w1.* FROM CorpProducts w1, (SELECT TOP(@PageIndex * @PageSize) row_number() OVER(ORDER BY Createtime DESC) num, id FROM CorpProducts where IsDel = 0 and IsShow = 1 and CorpId=@CorpId) w2  WHERE w1.id = w2.id AND w2.num > (@PageSize * (@PageIndex - 1))  ORDER BY w2.num ASC";
            string queryCountSql = $"SELECT COUNT(*) FROM [Ori_CloudWeb].[dbo].[CorpProducts] WHERE IsDel=0 and IsShow = 1 and CorpId=@CorpId;";

            return new ResponseResult<IEnumerable<CorpProductsDto>>(GetAll<CorpProductsDto>(sql, param), Count(queryCountSql, param));
        }

        //查询内容
        public ResponseResult<CorpProductsDto> GetProductContent(int id)
        {
            string sql = $"SELECT * FROM CorpProducts where IsDel = 0 and IsShow = 1 and id=@id ;";
            return new ResponseResult<CorpProductsDto>(Find<CorpProductsDto>(sql, new { id = id }));
        }

        #endregion
    }
}
