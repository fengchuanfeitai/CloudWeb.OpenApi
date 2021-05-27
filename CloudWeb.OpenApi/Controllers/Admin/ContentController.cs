using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
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
    [Route("api/admin/[controller]/[action]")]
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
        /// 查询所有内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ContentDto>> GetAll(SearchParam para)
        {
            return _service.GetAll(para);
        }

        /// <summary>
        /// 首页置顶
        /// </summary>
        /// <param name="defaultStatusParam">状态参数</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangeDefaultStatus(DefaultStatusParam defaultStatusParam)
        {
            return _service.ChangeDefaultStatus(defaultStatusParam);
        }

        /// <summary>
        /// 推荐首页状态
        /// </summary>
        /// <param name="publicStatusParam">状态参数</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangePublicStatus(PublicStatusParam publicStatusParam)
        {
            return _service.ChangePublicStatus(publicStatusParam);
        }

        /// <summary>
        ///  添加轮播
        /// </summary>
        /// <param name="topStatusParam">状态参数</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangeTopStatus(TopStatusParam topStatusParam)
        {
            return _service.ChangeTopStatus(topStatusParam);
        }

        /// <summary>
        /// 查询内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ContentDto> GetContent(int id)
        {
            return _service.GetContent(id);
        }

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ResponseResult<bool> EditContent(ContentDto contentDto)
        {
            return _service.EditContent(contentDto);
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> AddContent(ContentParam contentParam)
        {
            return _service.AddContent(contentParam);
        }

        /// <summary>
        /// 删除内容
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public ResponseResult<bool> DeleteContent(int[] ids)
        {
            return _service.DeleteContent(ids);
        }

    }
}
