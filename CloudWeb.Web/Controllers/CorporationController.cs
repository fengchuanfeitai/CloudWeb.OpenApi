using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.Web.Controllers
{
    public class CorporationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
