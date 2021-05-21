using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 用户操作
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public UserController(ILogger<UserController> logger, IUserService service)
        {
            _logger = logger;
            service = _service;
        }



    }
}
