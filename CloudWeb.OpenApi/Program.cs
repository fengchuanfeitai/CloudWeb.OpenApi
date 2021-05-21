using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Web;
using Autofac.Extensions.DependencyInjection;

namespace CloudWeb.OpenApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("��ʼ�� main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: �������ô���
                logger.Error(exception, "�����쳣��ֹͣ����");
                throw;
            }
            finally
            {
                // ȷ����Ӧ�ó����˳�֮ǰˢ�²�ֹͣ�ڲ���ʱ��/�̣߳�����Linux�ϳ��ֶַδ���
                NLog.LogManager.Shutdown();
            }

        }
        //ʹ��autofac
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
      .UseNLog();  // NLog: ����ע��Nlog
    }
}
