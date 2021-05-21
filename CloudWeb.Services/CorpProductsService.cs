using System.Collections.Generic;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;

namespace CloudWeb.Services
{
    public class CorpProductsService : BaseDao<CorpProductsDto>, ICorpProductsService
    {
        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        public ResponseResult<bool> AddProduct(CorpProductsDto corpProduct)
        {
            const string InsertSql = @"INSERT INTO dbo.CorpProducts
                 (CreateTime,ModifyTime,Creator,Modifier,[Name],Cover,Content,CorpId,IsPublic,IsDel)
                  VALUES
                 (@CreateTime,@ModifyTime,@Creator,@Modifier,@Name,@Cover,@Content,@CorpId,@IsPublic,@IsDel )";

            return new ResponseResult<bool>(Add(InsertSql, corpProduct));
        }

        /// <summary>
        /// 逻辑删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ResponseResult<bool> DelProduct(dynamic[] ids)
        {
            string DelSql = @"UPDATE dbo.CorpProducts SET IsDel =1 WHERE Id =@ids";
            if (ids.Length > 0)
            {
                DelSql = "UPDATE dbo.CorpProducts SET IsDel =1 WHERE Id In @ids";
            }

            return new ResponseResult<bool>(Update(DelSql, ids));
        }

        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateProduct(CorpProductsDto corpProduct)
        {
            var CorpProduct = GetProductsById(corpProduct.Id);
            if (!CorpProduct.IsSucceed)
                return new ResponseResult<bool>(false, CorpProduct.Message);
            if (CorpProduct.Result == null)
                return new ResponseResult<bool>(false, "产品不存在。");

            if (corpProduct.Name == CorpProduct.Result.Name &&
                corpProduct.Cover == CorpProduct.Result.Cover &&
                corpProduct.Content == CorpProduct.Result.Content &&
                corpProduct.CorpId == CorpProduct.Result.CorpId &&
                corpProduct.Sort == CorpProduct.Result.Sort &&
                corpProduct.IsDisplay == CorpProduct.Result.IsDisplay &&
                corpProduct.IsDel == CorpProduct.Result.IsDel)
                return new ResponseResult<bool>(true, "数据无更改");

            const string UpdateSql = @"UPDATE dbo.CorpProducts SET 
                  ModifyTime=@ModifyTime,Modifier=@Modifier,[Name]=@Name,Cover=@Cover,
                  Content=@Content,CorpId=@CorpId,Sort=@Sort,IsDisplay=@IsDisplay,IsDel=@IsDel 
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
            const string SelSql = @"SELECT Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],Cover,Content,CorpId,Sort,IsDisplay,IsDel
                                    FROM dbo.CorpProducts WHERE IsDel=0 AND CorpId=@corpId ORDER BY Sort DESC, CreateTime DESC ";

            return new ResponseResult<IEnumerable<CorpProductsDto>>(GetAll(SelSql, corpId));
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseResult<CorpProductsDto> GetProductsById(int id)
        {
            const string SelSql = @"SELECT Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],Cover,Content,CorpId,Sort,IsDisplay,IsDel
                                    FROM dbo.CorpProducts WHERE IsDel=0 AND Id=@id";

            return new ResponseResult<CorpProductsDto>(Find(SelSql, id));
        }

        /// <summary>
        /// 获取所有的产品信息
        /// </summary>
        /// <returns></returns>
        public ResponseResult<IEnumerable<CorpProductsDto>> GetProducts()
        {
            const string SelSql = @"SELECT Id,CreateTime,ModifyTime,Creator,ModifyTime,[Name],Cover,Content,CorpId,Sort,IsDisplay,IsDel
                                    FROM dbo.CorpProducts WHERE IsDel=0 ORDER BY Sort DESC, CreateTime DESC ";

            return new ResponseResult<IEnumerable<CorpProductsDto>>(GetAll(SelSql));
        }
    }
}
