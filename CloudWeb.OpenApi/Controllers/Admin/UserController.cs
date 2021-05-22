using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 用户展示
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AuthorizeController
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

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>bc
        [HttpPost]
        [AllowAnonymous]
        public ResponseResult<UserData> Login(string name, string password)
        {
            return _service.Login(name, password);
        }
    }
}
