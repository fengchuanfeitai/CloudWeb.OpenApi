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

        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}
