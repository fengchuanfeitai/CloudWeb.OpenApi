
using Microsoft.AspNetCore.Mvc;


namespace CloudWeb.Web.Controllers
{
    public class SiteInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
