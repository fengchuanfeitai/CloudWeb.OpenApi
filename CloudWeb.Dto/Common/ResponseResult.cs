namespace CloudWeb.Dto.Common
{
    /// <summary>
    ///  执行返回结果
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 成功返回码
        /// </summary>
        public static int Ok = (int)HttpStatusCode.OK;

        /// <summary>
        /// 失败返回码
        /// </summary>
        public int Fail = (int)HttpStatusCode.fail;

        public virtual ResponseResult Set(int status, string message)
        {
            code = status;
            msg = message;
            return this;
        }
        /// <summary>
        /// 设定错误信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual ResponseResult SetFailMessage(string message)
        {
            return Set(Fail, message);
        }
        public virtual ResponseResult SetFail()
        {
            return Set(Fail, string.Empty);
        }
        public ResponseResult(int code, string message)
        {
            Set(code, message);
        }
        /// <summary>
        /// 如果是给字符串，表示有错误信息，默认IsSucceed=false
        /// </summary>
        /// <param name="message"></param>
        public ResponseResult(string message)
        {
            Set(Fail, message);
        }
        /// <summary>
        /// 如果是空的，没有信息，默认IsSucceed=true
        /// </summary>
        public ResponseResult()
        {
        }



        /// <summary>
        /// 执行是否成功
        /// 默认为200
        /// </summary>
        public int code { get; set; } = Ok;

        /// <summary>
        /// 总数
        /// </summary>
        public int count { get; set; } = 0;
        /// <summary>
        /// 执行信息（一般是错误信息）
        /// 默认置空
        /// </summary>
        public string msg { get; set; } = string.Empty;
    }
    /// <summary>
    /// 执行返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseResult<T> : ResponseResult
    {
        public ResponseResult<T> Set(int status, int total, string message, T result)
        {
            count = total;
            code = status;
            msg = message;
            data = result;
            return this;
        }
        public ResponseResult<T> SetData(T data, int total = 0)
        {
            return Set(Ok, total, string.Empty, data);
        }

        public new ResponseResult<T> SetFail()
        {
            return Set(Fail, 0, string.Empty, default);
        }
        /// <summary>
        /// 设定错误信息
        /// 如果T正好也是string类型，可能set方法会存在用错的时候，所以取名SetMessage更明确
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public new ResponseResult<T> SetFailMessage(string message)
        {
            return Set(Fail, 0, message, default);
        }
        public ResponseResult()
        {
        }
        public ResponseResult(string message)
        {
            Set(Fail, message);
        }
        public ResponseResult(int status, string message)
        {
            Set(status, message);
        }
        public ResponseResult(T result, int total = 0)
        {
            SetData(result, total);
        }

        public T data { get; set; }
    }
}
