namespace CloudWeb.Dto.Param
{
    public class BaseConParam : BaseParam
    {
        /// <summary>
        /// 内容是否过滤Html
        /// </summary>
        public bool FilterHtml { get; set; }

        /// <summary>
        /// 标题截取长度
        /// </summary>
        public int? TitleCut { get; set; }

        /// <summary>
        /// 内容截取多少长度
        /// </summary>
        public int? ContentCut { get; set; }
    }

    /// <summary>
    /// 首页新闻列表Param
    /// </summary>
    public class IndexNewsParam : BaseConParam
    {
        /// <summary>
        /// 是否为轮播图
        /// </summary>
        public bool IsCarousel { get; set; }
    }

    /// <summary>
    /// （新闻报导/教学研究与论文/活动交流）列表Param
    /// </summary>
    public class ConSearchParam : BaseConParam
    {
        /// <summary>
        /// 传入一级栏目Id
        /// </summary>
        public int MasterId { get; set; }
        /// <summary>
        /// 学科
        /// </summary>
        public int? ObjId { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }
    }

    /// <summary>
    /// 根据栏目Id获取内容列表
    /// </summary>
    public class ConByColParam : BaseConParam
    {
        /// <summary>
        /// 栏目Id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 是否是轮播图
        /// </summary>
        public bool? IsCarousel { get; set; }
    }
}
