using CloudWeb.Dto;

namespace CloudWeb.IServices
{
    public interface ISiteInfoService : IBaseService<SiteInfoDto>
    {
        SiteInfoDto FindSiteInfoAsync();
    }
}
