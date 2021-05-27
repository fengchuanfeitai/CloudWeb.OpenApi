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
    /// 栏目管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/[controller]/[action]")]
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
        public ResponseResult<IEnumerable<ColumnDto>> GetAll(BaseParam pageParam)
        {
            return _service.GetAll(pageParam);
        }

        /// <summary>
        /// 改变显示状态
        /// </summary>
        /// <param name="showStatusParam">状态参数</param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            return _service.ChangeShowStatus(showStatusParam);
        }

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ColumnDto> GetColumn(int id)
        {
            return _service.GetColumn(id);
        }

        /// <summary>
        /// 修改栏目
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ResponseResult<bool> EditColumn(ColumnDto columnDto)
        {
            return _service.EditColumn(columnDto);
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
        public ResponseResult<bool> DeleteColumn(int[] ids)
        {
            return _service.DeleteColumn(ids);
        }

        /// <summary>
        /// 根据父Id获取子栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<ColumnDto>> GetColumnsByParent(int parentId)
        {
            return _service.GetColumnsByParent(parentId);
        }

        /// <summary>
        /// 获取栏目下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<IEnumerable<ColumnDropDownDto>> GetDropDownList(int id)
        {
            return _service.GetDropDownList(id);
        }
    }
}
