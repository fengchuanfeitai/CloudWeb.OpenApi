using Microsoft.AspNetCore.Mvc;

namespace CloudWeb.Web.Controllers
{
    public class ColumnController : Controller
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
