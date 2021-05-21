using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 公司相关Api
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CorporationController : Controller
    {
        private readonly ILogger<CorporationDto> _logger;
        private readonly ICorporationService _corporationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CorporationController(
            ILogger<CorporationDto> logger,
            ICorporationService corporationService)
        {

        }
    }
}
