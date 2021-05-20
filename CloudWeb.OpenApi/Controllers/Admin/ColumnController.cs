using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ColumnController : Controller
    {
        //初始化日志
        private readonly ILogger<ColumnController> _log;
        private readonly IColumnService _service;
        public ColumnController(ILogger<ColumnController> log, IColumnService service)
        {
            _log = log;
            _service = service;
        }

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ColumnDto>> GetAll()
        {
            return _service.GetAll();
        }
    }
}
