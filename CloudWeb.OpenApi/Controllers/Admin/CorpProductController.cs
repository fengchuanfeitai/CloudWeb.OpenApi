using CloudWeb.Dto;
using CloudWeb.Dto.Common;
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
        /// 查询所有未删除的产品
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<CorpProductsDto>> GetAll()
        {
            return _service.GetProducts();
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<CorpProductsDto> GetProductById(int id)
        {
            return _service.GetProductsById(id);
        }

        /// <summary>
        /// 获取公司产品
        /// </summary>
        /// <param name="corpId"></param>
        /// <returns></returns>
        [HttpGet]
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
        public ResponseResult<bool> addProduct(CorpProductsDto corpProduct)
        {
            return _service.AddProduct(corpProduct);
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> DelProduct(dynamic[] ids)
        {
            return _service.DelProduct(ids);
        }
    }
}
