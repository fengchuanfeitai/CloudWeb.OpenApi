﻿using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 栏目管理
    /// </summary>
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

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet("id", Name = nameof(GetColumn))]
        public ResponseResult<ColumnDto> GetColumn(int id)
        {
            return _service.GetColumn(id);
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ResponseResult<bool> EdittColumn(ColumnDto columnDto)
        {
            return _service.EdittColumn(columnDto);
        }

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> AddColumn(ColumnDto columnDto)
        {
            return _service.AddColumn(columnDto);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> DeleteColumn(dynamic[] ids)
        {
            return _service.DeleteColumn(ids);
        }
    }
}
