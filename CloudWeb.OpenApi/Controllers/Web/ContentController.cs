using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CloudWeb.Dto.Param;

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
        /// 获取首页新闻
        /// </summary>
        /// <param name="isCarousel">是否为轮播图</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetCarouselNews(bool isCarousel)
        {
            return _service.GetIndexNews(isCarousel);
        }

        /// <summary>
        /// 根据一级栏目Id及查询条件查询内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetContentByColumnId(SearchPapers param)
        {
            return _service.GetContentByColumnId(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ContentSelectDto> GetContent(int id)
        {
            return _service.GetContent(id);
        }
    }
}
