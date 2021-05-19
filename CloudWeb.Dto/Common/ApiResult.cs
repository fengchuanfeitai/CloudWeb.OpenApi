namespace CloudWeb.Dto.Common
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="T">数据泛型</typeparam>
    public class ApiResult<T>
    {
        public ApiResult()
        {
        }

        public ApiResult(int? count, T data, Meta meta)
        {
            Count = count;
            Data = data;
            Meta = meta;
        }

        /// <summary>
        /// 返回成功结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Succeed(T data = default(T))
        {
            var meta = new Meta()
            {
                Status = 200,
                Msg = ""
            };
            return new ApiResult<T>(null, data, meta);
        }

        /// <summary>
        /// 返回成功且带描述的结果
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Succeed(string msg, T data = default(T))
        {
            var meta = new Meta()
            {
                Status = 200,
                Msg = msg
            };
            return new ApiResult<T>(null, data, meta);
        }

        /// <summary>
        /// 返回错误结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> Fail(int code, string msg, T data = default(T))
        {
            var meta = new Meta()
            {
                Status = code,
                Msg = msg
            };
            return new ApiResult<T>(null, data, meta);
        }

        /// <summary>
        /// 总数
        /// </summary>
        public int? Count { get; set; }

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
