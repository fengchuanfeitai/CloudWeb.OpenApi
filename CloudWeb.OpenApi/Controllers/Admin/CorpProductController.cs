using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 公司产品相关的接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CorpProductController : Controller
    {
        private readonly ILogger<CorpProductsDto> _logger;
        private readonly ICorpProductsService _service;

        /// <summary>
        /// 构造函数
        /// </summary>      
        public CorpProductController(
            ILogger<CorpProductsDto> logger,
            ICorpProductsService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageList(ProductSearchParam pageParam)
        {
            return _service.GetPageProductList(pageParam);
        }


        /// <summary>
        /// 更改显示状态
        /// </summary>
        /// <param name="showStatusParam"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            return _service.ChangeShowStatus(showStatusParam);
        }


        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id", Name = nameof(GetProductById))]
        public ResponseResult<CorpProductsDto> GetProductById(int id)
        {
            return _service.GetProductsById(id);
        }

        /// <summary>
        /// 获取公司产品
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        [HttpGet("corpId", Name = nameof(GetProductByCorp))]
        public ResponseResult<IEnumerable<CorpProductsDto>> GetProductByCorp(int corpId)
        {
            return _service.GetProductsByCorpId(corpId);
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="corpProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> AddProduct(CorpProductsDto corpProduct)
        {
            if (corpProduct.Id == null) {
                return _service.AddProduct(corpProduct);
            }
            return _service.UpdateProduct(corpProduct);
            
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> DelProduct(int[] ids)
        {
            return _service.DelProduct(ids);
        }
    }
}
