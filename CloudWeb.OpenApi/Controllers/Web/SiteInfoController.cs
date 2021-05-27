using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CloudWeb.OpenApi.Controllers.Web
{
    /// <summary>
    /// 站点信息
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]
    public class SiteInfoController : Controller
    {
        private readonly ILogger<SiteInfoController> _logger;
        private readonly ISiteInfoService _siteInfoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="siteInfoService"></param>
        public SiteInfoController(ILogger<SiteInfoController> logger,
            ISiteInfoService siteInfoService)
        {
            _logger = logger;
            _siteInfoService = siteInfoService;
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<SiteInfoDto> GetSiteInfo()
        {
            return _siteInfoService.FindSiteInfo(); ;
        }
    }
}
