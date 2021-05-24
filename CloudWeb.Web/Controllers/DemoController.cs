using Microsoft.AspNetCore.Mvc;

namespace CloudWeb.Web.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult member_add()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }


        public IActionResult Password()
        {
            return View();
        }
    }
}
