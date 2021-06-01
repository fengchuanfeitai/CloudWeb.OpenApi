namespace CloudWeb.Dto.Param
{
    /// <summary>
    /// 基础查询参数
    /// </summary>
    public class BaseParam
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 一页展示数量
        /// </summary>
        public int PageSize { get; set; }
    }

    public class BaseWebParam : BaseParam
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

    public class FileParam
    {
        public string Path { get; set; }
    }

}
