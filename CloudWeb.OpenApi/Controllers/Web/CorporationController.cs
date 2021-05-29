using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CloudWeb.OpenApi.Controllers.Web
{
    /// <summary>
    /// web公司接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]
    public class CorporationController : Controller
    {
        //初始化日志
        private readonly ILogger<CorporationController> _log;
        private readonly ICorporationService _service;
        public CorporationController(ILogger<CorporationController> log, ICorporationService service)
        {
            _log = log;
            _service = service;
        }

        /// <summary>
        /// 查询所有公司
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<CorporationDto>> GetAllCorp()
        {
            return _service.GetAllCorp();
        }
    }
}
