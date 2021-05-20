using CloudWeb.Dto;
using CloudWeb.IServices;
using System;
using System.Linq;
using CloudWeb.DataRepository;
using CloudWeb.Dto.Common;

namespace CloudWeb.Services
{
    public class SiteInfoService : BaseDao<SiteInfoDto>, ISiteInfoService
    {
        /// <summary>
        /// 添加站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public ResponseResult<bool> Add(SiteInfoDto siteInfo)
        {
            const string sql = "insert into(CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo) values(@CreateTime,@ModifyTime,@Creator,@Modifier,@SiteTitle,@SiteKeyword,@SiteDesc,@SiteLogo,@CopyRight,@Icp,@Tel,@Address,@WeChatPublicNo)";

            ResponseResult<bool> result = new ResponseResult<bool>();
            return result.SetData(Add(sql, siteInfo));
        }

        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <returns></returns>
        public ResponseResult<SiteInfoDto> FindSiteInfo()
        {
            const string sql = "SELECT TOP 1 Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo FROM dbo.SiteInfo";

            ResponseResult<SiteInfoDto> result = new ResponseResult<SiteInfoDto>();
            return result.SetData(Find(sql));
        }


        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public ResponseResult<bool> Update(SiteInfoDto siteInfo)
        {
            const string sql = "UPDATE dbo.SiteInfo SET ModifyTime=@ModifyTime,Modifier=@Modifier,SiteTitle=@SiteTitle,SiteKeyword=@SiteKeyword,SiteDesc=@SiteDesc,SiteLogo=@SiteLogo,CopyRight=@CopyRight,Icp=@Icp,Tel=@Tel,[Address]=@Address,WeChatPublicNo=@WeChatPublicNo";
            ResponseResult<bool> result = new ResponseResult<bool>();
            return result.SetData(Update(sql, siteInfo));
        }

    }
}
