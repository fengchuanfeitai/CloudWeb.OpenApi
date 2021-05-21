using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    [Route("[controller]")]
    [Authorize]
    public class AuthorizeController : Controller
    {
      
    }
}
