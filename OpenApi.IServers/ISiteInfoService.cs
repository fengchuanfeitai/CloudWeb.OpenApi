using CloudWeb.Dto;
using CloudWeb.Dto.Common;

namespace CloudWeb.IServices
{
    public interface ISiteInfoService : IBaseService
    {
        ResponseResult<bool> Add(SiteInfoDto siteInfo);
        ResponseResult<SiteInfoDto> FindSiteInfo();

        ResponseResult<bool> Update(SiteInfoDto siteInfo);
    }
}
