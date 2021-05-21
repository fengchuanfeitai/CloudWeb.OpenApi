using CloudWeb.Dto;
using CloudWeb.IServices;
using CloudWeb.DataRepository;
using CloudWeb.Dto.Common;

namespace CloudWeb.Services
{
    public class SiteInfoService : BaseDao<SiteInfoDto>, ISiteInfoService
    {
        #region 私有方法

        private ResponseResult<SiteInfoDto> FindSiteInfoById(int id)
        {
            const string sql = @"SELECT TOP 1 
              Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,
              SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo 
              FROM dbo.SiteInfo 
              WHERE IsDel = 0 AND Id = @id";

            return new ResponseResult<SiteInfoDto>(Find(sql, id));
        }
        #endregion

        /// <summary>
        /// 添加站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public ResponseResult<bool> AddSiteInfo(SiteInfoDto siteInfo)
        {
            const string InsertSql = @"INSERT dbo.SiteInfo(CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,SiteDesc,
               SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo) 
               VALUES
              (@CreateTime,@ModifyTime,@Creator,@Modifier,@SiteTitle,@SiteKeyword,
               @SiteDesc,@SiteLogo,@CopyRight,@Icp,@Tel,@Address,@WeChatPublicNo)";

            return new ResponseResult<bool>(Add(InsertSql, siteInfo));
        }


        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="siteInfo">站点信息</param>
        /// <returns></returns>
        public ResponseResult<bool> UpdateSiteInfo(SiteInfoDto siteInfo)
        {
            var SiteInfo = FindSiteInfoById(siteInfo.Id);
            if (SiteInfo == null)
                return new ResponseResult<bool>(201, "站点信息不存在");

            if (siteInfo.SiteTitle == SiteInfo.data.SiteTitle &&
               siteInfo.SiteKeyword == SiteInfo.data.SiteKeyword &&
               siteInfo.SiteDesc == SiteInfo.data.SiteDesc &&
               siteInfo.SiteLogo == SiteInfo.data.SiteLogo &&
               siteInfo.CopyRight == SiteInfo.data.CopyRight &&
               siteInfo.Icp == SiteInfo.data.Icp &&
               siteInfo.Tel == SiteInfo.data.Tel &&
               siteInfo.Address == SiteInfo.data.Address &&
               siteInfo.WeChatPublicNo == SiteInfo.data.WeChatPublicNo)
                return new ResponseResult<bool>(201, "");

            const string sql = @"UPDATE dbo.SiteInfo SET
                  ModifyTime=@ModifyTime,Modifier=@Modifier,SiteTitle=@SiteTitle,SiteKeyword=@SiteKeyword,SiteDesc=@SiteDesc,
                  SiteLogo=@SiteLogo,CopyRight=@CopyRight,Icp=@Icp,Tel=@Tel,[Address]=@Address,WeChatPublicNo=@WeChatPublicNo
                  WHERE IsDel=0";

            return new ResponseResult<bool>(Update(sql, siteInfo));
        }

        /// <summary>
        /// 查询站点信息
        /// </summary>
        /// <returns></returns>
        public ResponseResult<SiteInfoDto> FindSiteInfo()
        {
            const string sql = @"SELECT TOP 1 
              Id,CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,
              SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo 
              FROM dbo.SiteInfo";

            var SiteInfoResult = new ResponseResult<SiteInfoDto>(Find(sql));

            if (SiteInfoResult.data == null)
            {
                var NewSiteInfo = new SiteInfoDto()
                {
                    Creator = 0,
                    Modifier = 0,
                    SiteTitle = "-1",
                    SiteKeyword = "-2",
                    SiteDesc = "-3",
                    SiteLogo = "-4",
                    CopyRight = "-5",
                    Icp = "-6",
                    Tel = "-7",
                    Address = "-8",
                    WeChatPublicNo = "-9"
                };
                var AddResult = AddSiteInfo(NewSiteInfo);
                if (AddResult.code != 200)
                    return new ResponseResult<SiteInfoDto>(201, "初始化站点信息失败！");
                FindSiteInfo();
            }

            return SiteInfoResult;
        }
    }
}
