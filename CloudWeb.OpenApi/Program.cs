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
                }).UseNLog();  // NLog: 依赖注入Nlog
    }
}