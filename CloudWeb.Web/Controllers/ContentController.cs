using Microsoft.AspNetCore.Mvc;

namespace CloudWeb.Web.Controllers
{
    public class ContentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
