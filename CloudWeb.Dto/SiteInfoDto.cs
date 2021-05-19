
using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class SiteInfoDto : BaseDto
    {
        /// <summary>
        /// 站点标题
        /// </summary>
        public string SiteTitle { get; set; }

        /// <summary>
        /// 站点关键字
        /// </summary>
        public string SiteKeyword { get; set; }

        /// <summary>
        /// 站点描述
        /// </summary>
        public string SiteDesc { get; set; }

        /// <summary>
        /// 站点Logo
        /// </summary>
        public string SiteLogo { get; set; }

        /// <summary>
        /// 站点版权
        /// </summary>
        public string CopyRight { get; set; }

        /// <summary>
        /// 站点备案号
        /// </summary>
        public string Icp { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 微信公众号
        /// </summary>
        public string WeChatPublicNo { get; set; }

    }
}
