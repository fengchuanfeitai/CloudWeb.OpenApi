using System.Collections.Generic;

namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDto<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> date { get; set; }

    }
}
