using System;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;

namespace CloudWeb.OpenApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("初始化 main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: 捕获设置错误
                logger.Error(exception, "由于异常而停止程序");
                throw;
            }
            finally
            {
                // 确保在应用程序退出之前刷新并停止内部计时器/线程（避免Linux上出现分段错误）
                NLog.LogManager.Shutdown();
            }

        }
        //使用autofac
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    // logging.ClearProviders(); // 这个方法会清空所有控制台的输出
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                }).UseNLog(); // 使用NLog

    }
}