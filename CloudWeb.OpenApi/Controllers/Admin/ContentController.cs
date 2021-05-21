using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 内容管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/[controller]")]
    public class ContentController : Controller
    {

        //初始化日志
        private readonly ILogger<ContentController> _log;
        private readonly IContentService _service;
        public ContentController(ILogger<ContentController> log, IContentService service)
        {
            _log = log;
            _service = service;
        }

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetAll()
        {
            return _service.GetAll();
        }

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet("id", Name = nameof(GetContent))]
        public ResponseResult<ContentDto> GetContent(int id)
        {
            return _service.GetContent(id);
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ResponseResult<bool> EdittContent([FromBody] ContentDto contentDto)
        {
            return _service.EdittContent(contentDto);
        }

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> AddContent([FromBody] ContentDto contentDto)
        {
            return _service.AddContent(contentDto);
        }

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> DeleteContent(dynamic[] ids)
        {
            return _service.DeleteContent(ids);
        }

    }
}
