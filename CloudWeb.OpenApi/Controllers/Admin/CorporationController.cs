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
    /// 公司相关接口
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CorporationController : Controller
    {
        private readonly ILogger<CorporationDto> _logger;
        private readonly ICorporationService _corporationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CorporationController(
            ILogger<CorporationDto> logger,
            ICorporationService corporationService)
        {
            _logger = logger;
            _corporationService = corporationService;
        }

        /// <summary>
        /// 分页查询公司列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<CorporationDto>> GetPageList(CorpSearchParam pageParam)
        {
            return _corporationService.GetPageList(pageParam);
        }

        /// <summary>
        /// 更改显示状态
        /// </summary>
        /// <param name="showStatusParam"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ChangeShowStatus(ShowStatusParam showStatusParam)
        {
            return _corporationService.ChangeShowStatus(showStatusParam);
        }

        /// <summary>
        /// 查询公司信息
        /// </summary>
        /// <param name="id">公司id</param>
        /// <returns></returns>
        [HttpGet("id", Name = nameof(GetCorporation))]
        public ResponseResult<CorporationDto> GetCorporation(int id)
        {
            return _corporationService.GetCorporation(id);
        }

        /// <summary>
        /// 添加公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> AddCorporaion(CorporationDto corporation)
        {
            if (corporation.CorpId == null)
                return _corporationService.AddCorporation(corporation);
            return _corporationService.UpdateCorporation(corporation);
        }

        /// <summary>
        /// 删除公司信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<bool> DelCorporation(int[] ids)
        {
            return _corporationService.DelCorporation(ids);
        }

        /// <summary>
        /// 获取公司下拉框列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<IEnumerable<CorporationDto>> GetCorpSelectList()
        {
            return _corporationService.GetCorpSelectList();
        }
    }
}
