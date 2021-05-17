namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 返回结果共用
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }
}
