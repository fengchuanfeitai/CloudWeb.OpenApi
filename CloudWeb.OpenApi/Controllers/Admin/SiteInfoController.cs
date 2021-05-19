using CloudWeb.Dto;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 网站站点设置
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
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
        public IActionResult GetSiteInfo(int id)
        {
            SiteInfoDto dto = _siteInfoService.FindSiteInfoAsync();
            if (dto == null)
            {
                return NotFound();
            }
            return new ObjectResult(dto);
        }

        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut()]
        public IActionResult UpdateSiteInfo([FromBody] SiteInfoDto dto)
        {
            if (dto == null)
                return BadRequest();

            SiteInfoDto siteInfo = _siteInfoService.FindSiteInfoAsync();
            if (siteInfo == null)
            {
                return NotFound();
            }

            _siteInfoService.UpdateAsync(dto);
            return 
        }
    }
}
