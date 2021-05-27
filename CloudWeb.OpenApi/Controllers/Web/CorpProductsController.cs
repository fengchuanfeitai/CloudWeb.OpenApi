using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.Controllers.Web
{

    /// <summary>
    /// web内容接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]

    public class CorpProductsController : Controller
    {

        //初始化日志
        private readonly ILogger<CorpProductsController> _log;
        private readonly ICorpProductsService _service;

        public CorpProductsController(ILogger<CorpProductsController> log, ICorpProductsService service)
        {
            _log = log;
            _service = service;
        }

        /// <summary>
        /// 查询产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<IEnumerable<CorpProductsDto>> GetPageProduct(int id)
        {
            return _service.GetPageProduct(id);
        }
    }
}
