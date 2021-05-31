using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.DataRepository;
using CloudWeb.Dto.Common;
using System;

namespace CloudWeb.Services
{
    public class SiteInfoService : BaseDao, ISiteInfoService
    {
        #region 私有方法

        private ResponseResult<SiteInfoDto> FindSiteInfoById(int id)
        {
            const string sql = @"SELECT TOP 1 
              Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,
              SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo 
              FROM dbo.SiteInfo 
              WHERE Id = @id";

            return new ResponseResult<SiteInfoDto>(Find<SiteInfoDto>(sql, new { id = id }));
        }

        /// <summary>
        /// 添加站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        private ResponseResult<bool> AddSiteInfo(SiteInfoDto siteInfo)
        {
            const string InsertSql = @"INSERT dbo.SiteInfo(CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,
               SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo) 
               VALUES
              (@CreateTime,@ModifyTime,@Creator,@Modifier,@SiteTitle,@SiteKeyword,
               @SiteDesc,@SiteLogo,@CopyRight,@Icp,@Tel,@Address,@WeChatPublicNo)";

            return new ResponseResult<bool>(Add(InsertSql, siteInfo));
        }

        #endregion

        #region 公用方法

        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <returns></returns>
        public ResponseResult<SiteInfoDto> FindSiteInfo()
        {
            var Result = new ResponseResult<SiteInfoDto>();
            const string sql = @"SELECT TOP 1 
              Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,
              SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo 
              FROM dbo.SiteInfo";

            var SiteInfo = Find<SiteInfoDto>(sql);
            if (SiteInfo == null)
            {
                var NewSiteInfo = new SiteInfoDto()
                {
                    Creator = 1,
                    Modifier = 1,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    SiteTitle = "网站标题",
                    SiteKeyword = "网站关键字",
                    SiteDesc = "网站描述",
                    SiteLogo = "#",
                    CopyRight = "网站版权信息",
                    Icp = "网站备案号",
                    Tel = "联系方式",
                    Address = "地址",
                    WeChatPublicNo = "#"
                };
                var AddResult = AddSiteInfo(NewSiteInfo);
                if (AddResult.code != ResponseResult.Ok)
                    return Result.SetFailMessage("初始化站点信息失败！");
                FindSiteInfo();
            }

            return Result.SetData(SiteInfo);
        }

        #endregion

        #region 后台逻辑接口

        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateSiteInfo(SiteInfoDto siteInfo)
        {
            var Result = new ResponseResult<bool>();

            var SiteInfoRes = FindSiteInfoById(siteInfo.Id);
            if (SiteInfoRes.data == null)
                return Result.SetFailMessage("站点信息不存在");

            if (Equals(siteInfo, SiteInfoRes.data))
                return Result.Set(ResponseResult.Ok, 0, "", true);

            siteInfo.Modifier = siteInfo.Creator;
            siteInfo.ModifyTime = DateTime.Now;

            const string sql = @"UPDATE dbo.SiteInfo SET                  ModifyTime=@ModifyTime,Modifier=@Modifier,SiteTitle=@SiteTitle,
              SiteKeyword=@SiteKeyword,SiteDesc=@SiteDesc,
              SiteLogo=@SiteLogo,CopyRight=@CopyRight,Icp=@Icp,Tel=@Tel,
              [Address]=@Address,WeChatPublicNo=@WeChatPublicNo";

            return Result.SetData(Update(sql, siteInfo));
        }

        #endregion       
    }
}
