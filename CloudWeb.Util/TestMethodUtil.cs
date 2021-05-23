using Microsoft.Extensions.Logging;
using System;

namespace CloudWeb.Util
{
    /// <summary>
    /// 计算执行方法，辅助类 
    /// </summary>
    public class TestMethodUtil
    {
        private readonly ILogger<TestMethodUtil> _logger;

        public TestMethodUtil(ILogger<TestMethodUtil> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取方法运行耗时
        /// </summary>
        /// <param name="_method">委托方法</param>
        /// <param name="return_Obj">执行委托方法后的返回值</param>
        /// <param name="_params">变长参数</param>
        /// <returns>该方法返回测量的毫秒值</returns>
        public Double getMethodRuntime(Delegate _method, out Object return_Obj, params Object[] _params)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //执行委托
            if (_params.Length == 0)
            {
                //开始测量
                stopwatch.Start();
                //动态调用
                return_Obj = _method.DynamicInvoke();

                string methodName = _method.Method.Name;
                //结束测量
                stopwatch.Stop();
            }
            else
            {
                //开始测量
                stopwatch.Start();

                return_Obj = _method.DynamicInvoke(_params);
                //结束测量
                stopwatch.Stop();
            }
            //此字段不严谨
            //return stopwatch.ElapsedMilliseconds;
            TimeSpan timeSpan = stopwatch.Elapsed;
            //记录执行方式时间
            _logger.LogInformation($"执行方法：{_method.Method.Name}，总共花费{timeSpan.TotalSeconds}秒");
            //获取秒值
            return timeSpan.TotalSeconds;
        }

    }
}
