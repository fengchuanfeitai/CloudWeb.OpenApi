using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UEditor.Core;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    public class UEditorController : Controller
    {
        private readonly UEditorService _ueditorService;
        public UEditorController(UEditorService ueditorService)
        {
            this._ueditorService = ueditorService;
        }

        //如果是API，可以按MVC的方式特别指定一下API的URI
        [HttpPost]
        [Produces("application/json")]
        [Route("api/admin/uploads")]
        [HttpGet, HttpPost]
        public ContentResult Upload()
        {
            var response = _ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }
    }
}
