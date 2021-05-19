using CloudWeb.Dto;
using CloudWeb.IServices;
using System;
using System.Linq;
using CloudWeb.DataRepository;

namespace CloudWeb.Services
{
    public class SiteInfoService : BaseDao<SiteInfoDto>, ISiteInfoService
    {
        /// <summary>
        /// 添加站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public bool AddAsync(SiteInfoDto siteInfo)
        {
            string sql = "insert into(CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo) values(@CreateTime,@ModifyTime,@Creator,@Modifier,@SiteTitle,@SiteKeyword,@SiteDesc,@SiteLogo,@CopyRight,@Icp,@Tel,@Address,@WeChatPublicNo)";
            return Add(sql, siteInfo);
        }

        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <returns></returns>
        public SiteInfoDto FindSiteInfoAsync()
        {
            string sql = "SELECT TOP 1 Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo FROM dbo.SiteInfo";

            return Find(sql);
        }

        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public bool UpdateAsync(SiteInfoDto siteInfo)
        {
            string sql = "UPDATE dbo.SiteInfo SET ModifyTime=@ModifyTime,Modifier=@Modifier,SiteTitle=@SiteTitle,SiteKeyword=@SiteKeyword,SiteDesc=@SiteDesc,SiteLogo=@SiteLogo,CopyRight=@CopyRight,Icp=@Icp,Tel=@Tel,[Address]=@Address,WeChatPublicNo=@WeChatPublicNo";

            return Update(sql, siteInfo);
        }

        public IQueryable<SiteInfoDto> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAsync(dynamic[] ids)
        {
            throw new NotImplementedException();
        }
        public SiteInfoDto FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
