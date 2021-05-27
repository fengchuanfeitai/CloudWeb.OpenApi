using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CloudWeb.OpenApi.Controllers.Web
{
    /// <summary>
    /// web内容接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]
    public class ContentController : Controller
    {
        //初始化日志
        private readonly ILogger<ContentController> _log;
        private readonly IContentService _service;
        public ContentController(ILogger<ContentController> log, IContentService service)
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
