using CloudWeb.IServices;
using CloudWeb.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 登录相关
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userServier;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService"></param>
        public LoginController(
            ILogger<LoginController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userServier = userService;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        [HttpGet("verifyImage")]
        public HttpResponseMessage VerifyImage()
        {
            var validate = new ValidateCodeUtil();
            string code = validate.CreateValidateCode(4);
            byte[] bytes = validate.CreateValidateGraphic(code);
            //从图片中读取流           
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)                
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return resp;
        }
    }
}
