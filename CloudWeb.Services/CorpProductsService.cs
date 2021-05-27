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

        private int GetSort()
        {
            var MaxSortsql = "SELECT MAX(Sort) FROM dbo.CorpProducts";
            return MaxSort(MaxSortsql) + 1;
        }

        #endregion

        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddProduct(CorpProductsDto corpProduct)
        {
            corpProduct.CreateTime = DateTime.Now;
            corpProduct.ModifyTime = corpProduct.CreateTime;
            corpProduct.Creator = 0;
            corpProduct.Modifier = corpProduct.Modifier;
            if (corpProduct.Sort == null)
                corpProduct.Sort = GetSort();

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
            corpProduct.Modifier = 0;
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
        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageProductList(BaseParam pageParam)
        {
            const string SelSql = @"SELECT cp1.Id,CreateTime,ModifyTime,Creator,ModifyTime,
                        [Name],Cover,Content,LocationUrl,CorpId,Sort,IsShow,IsDel 
                        FROM dbo.CorpProducts cp1,
                        (SELECT TOP (@PageIndex * @Pagesize) ROW_NUMBER() OVER(ORDER BY CreateTime DESC) AS [Index],Id
                        FROM CorpProducts) cp2
                        WHERE cp1.Id = cp2.Id AND cp1.IsDel=0 AND cp2.[Index] > (@PageSize * (@PageIndex - 1))
                        ORDER BY cp2.[Index] asc";

            var List = GetAll<CorpProductsDto>(SelSql, pageParam);
            foreach (var item in List)
            {
                //获取公司名
                item.CorpName = GetCorpName(item.CorpId);
            }
            const string CountSql = @"SELECT COUNT(*) FROM dbo.CorpProducts WHERE IsDel=0";
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

        #region 网站接口


        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageProduct(int id)
        {
            string sql = @"SELECT * FROM dbo.CorpProducts WHERE IsDel=0 and CorpId=@id";
            return new ResponseResult<IEnumerable<CorpProductsDto>>(GetAll<CorpProductsDto>(sql, new { id = id }));
        }

        #endregion
    }
}
