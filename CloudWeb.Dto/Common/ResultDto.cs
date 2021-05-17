namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T">数据泛型</typeparam>
    public class ResultDto<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 状态信息
        /// </summary>
        public Meta Meta { get; set; }
    }
}
