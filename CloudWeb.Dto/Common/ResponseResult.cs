namespace CloudWeb.Dto.Common
{
    /// <summary>
    ///  执行返回结果
    /// </summary>
    public class ResponseResult
    {
        public virtual ResponseResult Set(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
            return this;
        }
        /// <summary>
        /// 设定错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual ResponseResult SetFailMessage(string message)
        {
            return Set(false, message);
        }
        public virtual ResponseResult SetFail()
        {
            return Set(false, string.Empty);
        }
        public ResponseResult(bool isSucceed, string message)
        {
            Set(isSucceed, message);
        }
        /// <summary>
        /// 如果是给字符串，表示有错误信息，默认IsSucceed=false
        /// </summary>
        /// <param name="message"></param>
        public ResponseResult(string message)
        {
            Set(false, message);
        }
        /// <summary>
        /// 如果是空的，没有信息，默认IsSucceed=true
        /// </summary>
        public ResponseResult()
        {
        }
        /// <summary>
        /// 执行是否成功
        /// 默认为True
        /// </summary>
        public bool IsSucceed { get; set; } = true;
        /// <summary>
        /// 执行信息（一般是错误信息）
        /// 默认置空
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
    /// <summary>
    /// 执行返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : ResponseResult
    {
        public ResponseResult<T> Set(bool isSucceed, string message, T result)
        {
            IsSucceed = isSucceed;
            Message = message;
            Result = result;
            return this;
        }
        public ResponseResult<T> SetData(T data)
        {
            return Set(true, string.Empty, data);
        }
        public new ResponseResult<T> SetFail()
        {
            return Set(false, string.Empty, default);
        }
        /// <summary>
        /// 设定错误信息
        /// 如果T正好也是string类型，可能set方法会存在用错的时候，所以取名SetMessage更明确
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public new ResponseResult<T> SetFailMessage(string message)
        {
            return Set(false, message, default);
        }
        public ResponseResult()
        {
        }
        public ResponseResult(string message)
        {
            Set(false, message);
        }
        public ResponseResult(bool isSucceed, string message)
        {
            Set(isSucceed, message);
        }
        public ResponseResult(T result)
        {
            SetData(result);
        }

        public T Result { get; set; }
    }
}
