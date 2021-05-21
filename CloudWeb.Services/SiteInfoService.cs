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
        public ResponseResult<bool> Add(SiteInfoDto siteInfo)
        {
            const string InsertSql = @"INSERT INTO
                  (CreateTime,ModifyTime,Creator,Modifier,SiteTitle,SiteKeyword,
                   SiteDesc,SiteLogo,CopyRight,Icp,Tel,[Address],WeChatPublicNo) 
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
        public ResponseResult<bool> Update(SiteInfoDto siteInfo)
        {
            var SiteInfo = FindSiteInfoById(siteInfo.Id);
            if (SiteInfo == null)
                return new ResponseResult<bool>(false, "站点信息不存在");

            if (siteInfo.SiteTitle == SiteInfo.Result.SiteTitle &&
               siteInfo.SiteKeyword == SiteInfo.Result.SiteKeyword &&
               siteInfo.SiteDesc == SiteInfo.Result.SiteDesc &&
               siteInfo.SiteLogo == SiteInfo.Result.SiteLogo &&
               siteInfo.CopyRight == SiteInfo.Result.CopyRight &&
               siteInfo.Icp == SiteInfo.Result.Icp &&
               siteInfo.Tel == SiteInfo.Result.Tel &&
               siteInfo.Address == SiteInfo.Result.Address &&
               siteInfo.WeChatPublicNo == SiteInfo.Result.WeChatPublicNo)
                return new ResponseResult<bool>(true, "");

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

            return new ResponseResult<SiteInfoDto>(Find(sql));
        }
    }
}
