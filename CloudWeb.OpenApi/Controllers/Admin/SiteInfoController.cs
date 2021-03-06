using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 网站站点api
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class SiteInfoController : Controller
    {
        private readonly ILogger<SiteInfoController> _logger;
        private readonly ISiteInfoService _siteInfoService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteInfoController(ILogger<SiteInfoController> loger,
            ISiteInfoService siteInfoService)
        {
            _logger = loger;
            _siteInfoService = siteInfoService;
        }


        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetSiteInfo))]
        public ResponseResult<SiteInfoDto> GetSiteInfo()
        {
            return _siteInfoService.FindSiteInfo();
        }

        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> UpdateSiteInfo(SiteInfoDto dto)
        {
            return _siteInfoService.UpdateSiteInfo(dto);
        }        
    }
}
