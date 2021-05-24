using Microsoft.AspNetCore.Mvc;

namespace CloudWeb.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
