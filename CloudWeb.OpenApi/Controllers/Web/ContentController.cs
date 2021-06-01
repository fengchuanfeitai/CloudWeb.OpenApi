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
        /// <param name="param"></param>       
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetIndexNews(IndexNewsParam param)
        {
            return _service.GetIndexNews(param);
        }

        /// <summary>
        /// 获取（新闻报导/教学研究与论文/活动交流）内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>        
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetColContent(ConSearchParam param)
        {
            return _service.GetColPageContent(param);
        }

        /// <summary>
        /// 根据columnId 获取内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetContentBycId(ConByColParam param)
        {
            return _service.GetConByCol(param);
        }

        /// <summary>
        /// 根据id查询内容
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
