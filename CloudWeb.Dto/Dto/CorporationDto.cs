using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class CorporationDto : BaseDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int? CorpId { get; set; }

        /// <summary>
        /// 显示的图片
        /// </summary>
        public int DisPlayIndex { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 截取后的公司名称
        /// </summary>
        public string TruncatName { get; set; }

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
        public string ColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string ColTxtName { get; set; }

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
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }
    }
}
