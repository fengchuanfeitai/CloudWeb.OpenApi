using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<IEnumerable<ContentDto>> GetCarouselNews()
        {
            return _service.GetCarouselNews();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<IEnumerable<ContentDto>> GetDefaultNews()
        {
            return _service.GetCarouselNews();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<IEnumerable<ContentDto>> GetWebContent()
        {
            return _service.GetCarouselNews();
        }
    }
}
