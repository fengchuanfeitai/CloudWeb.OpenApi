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
        public int page { get; set; }

        /// <summary>
        /// 一页展示数量
        /// </summary>
        public int limit { get; set; }
    }
}
