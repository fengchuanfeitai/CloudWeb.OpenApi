using CloudWeb.Dto;
using CloudWeb.Dto.Common;

namespace CloudWeb.IServices
{
    public interface ISiteInfoService : IBaseService
    {
        /// <summary>
        /// 添加站点信息
        /// </summary>
        /// <param name="siteInfo"></param>
        /// <returns></returns>
        ResponseResult<bool> Add(SiteInfoDto siteInfo);

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="siteInfo"></param>
        /// <returns></returns>
        ResponseResult<bool> Update(SiteInfoDto siteInfo);

        /// <summary>
        /// 查询公司信息
        /// </summary>
        /// <returns></returns>
        ResponseResult<SiteInfoDto> FindSiteInfo();
    }
}
