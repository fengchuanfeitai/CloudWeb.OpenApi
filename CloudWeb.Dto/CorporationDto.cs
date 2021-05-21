using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class CorporationDto : BaseDto
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CorpId { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司封面图
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 灰色的Logo图
        /// </summary>
        public string Logo1 { get; set; }

        /// <summary>
        /// 正常的Logo图
        /// </summary>
        public string Logo2 { get; set; }

        /// <summary>
        /// 栏目Id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 关于我们
        /// </summary>
        public string AboutUs { get; set; }

        /// <summary>
        /// 关于我们图片
        /// </summary>
        public string AboutUsCover { get; set; }

        /// <summary>
        /// 联系我们
        /// </summary>
        public string ContactUs { get; set; }

        /// <summary>
        /// 联系我们背景图
        /// </summary>
        public string ContactUsBg { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
