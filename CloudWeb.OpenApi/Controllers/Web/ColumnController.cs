using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudWeb.OpenApi.Controllers.Web
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]
    public class ColumnController : Controller
    {
        //初始化日志
        private readonly ILogger<ColumnController> _log;
        private readonly IColumnService _service;
        public ColumnController(ILogger<ColumnController> log, IColumnService service)
        {
            _log = log;
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
