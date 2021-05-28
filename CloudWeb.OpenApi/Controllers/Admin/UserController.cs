using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;
using CloudWeb.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 用户展示
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
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
            _service = service;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>bc
        [HttpPost]
        //[AllowAnonymous]
        public ResponseResult<UserData> Login(UserParam user)
        {
            //string code = HttpContext.Session.GetString("LoginValidateCode");
            var code = HttpContext.Request.Cookies["LoginValidateCode"];

            if (code != user.VerifyCode)
            {
                return new ResponseResult<UserData>("请输入正确的验证码");
            }
            return _service.Login(user);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult VerifyImage()
        {
            var validate = new ValidateCodeUtil();
            string code = validate.CreateValidateCode(4);
            HttpContext.Response.Cookies.Append("LoginValidateCode", code);
            byte[] bytes = validate.CreateValidateGraphic(code);

            var file = File(bytes, @"image/jpeg");
            //验证码写入cookie
            return file;
        }
    }
}
