using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 用户登录验证
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public class AuthorizeController : Controller
    {
      
    }
}
