using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace CloudWeb.OpenApi.Core.Core
{
    public static class CoreHttpContext
    {
        private static Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnviroment;
        public static string WebPath => _hostEnviroment.WebRootPath;

        public static string MapPath(string path)
        {
            return Path.Combine(_hostEnviroment.WebRootPath, path);
        }

        internal static void Configure(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
        }
    }
    public static class StaticHostEnviromentExtensions
    {
        public static IApplicationBuilder UseStaticHostEnviroment(this IApplicationBuilder app)
        {
            var webHostEnvironment = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            CoreHttpContext.Configure(webHostEnvironment);
            return app;
        }
    }
}
