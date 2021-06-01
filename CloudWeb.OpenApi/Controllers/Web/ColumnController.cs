using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Dto;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CloudWeb.OpenApi.Controllers.Web
{
    /// <summary>
    /// 栏目管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/web/[controller]/[action]")]
    public class ColumnController : Controller
    {
        //初始化日志
        private readonly ILogger<ColumnController> _log;
        private readonly IColumnService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="service"></param>
        public ColumnController(ILogger<ColumnController> log, IColumnService service)
        {
            _log = log;
            _service = service;
        }

        #region 网站接口

        /// <summary>
        /// 根据ParenId 获取子栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParentId(int parentId)
        {
            return _service.GetColumnsByParentId(parentId);
        }

        /// <summary>
        /// 获取栏目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ColumnSelectDto> GetColumnById(int id)
        {
            return _service.GetColumn(id);
        }

        /// <summary>
        /// 获取栏目轮播图
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IList<CarouselDto>> GetCarouselImg(int columnId)
        {
            return _service.GetCarouselImg(columnId);
        }

        /// <summary>
        /// 获取实验云下菜单（level = 3）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ColumnDto>> GetExperimentCol(int columnId, int level)
        {
            return _service.GetExperimentCol(columnId, level);
        }

        #endregion
    }
}
