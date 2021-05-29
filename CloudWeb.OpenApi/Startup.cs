using CloudWeb.IServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using CloudWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using CloudWeb.OpenApi.Filters;
using Autofac;
using CloudWeb.OpenApi.Core.Aop;
using CloudWeb.Util;
using CloudWeb.OpenApi.Core.Jwt;
using NLog.Extensions.Logging;
using CloudWeb.OpenApi.Core.Core;
using Microsoft.Extensions.FileProviders;
using UEditor.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using CloudWeb.Dto.Common;

namespace CloudWeb.OpenApi
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        //���autofac��DI��������
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע��IUserService��UserService�ӿڼ���Ӧ��ʵ����
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ColumnService>().As<IColumnService>().InstancePerLifetimeScope();
            builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();

            //ע��aop������ 
            //��ҵ���������ƴ��˽�ȥ����ҵ���ӿں�ʵ������ע�ᣬҲ��ҵ�������������˴���
            builder.AddAopService(ServiceExtensions.GetAssemblyName());
        }  //����ע��

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => false;//����Ҫ��Ϊfalse��Ĭ����true��true��ʱ��session��Ч
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            // ָ��Session���淽ʽ:�ַ��ڴ滺��
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie();
            //services.AddSession();
            services.AddControllers(options =>
            {
                //ע��ͳһ������أ��쳣��������ģ����֤
                options.Filters.Add<ModelValidateActionFilterAttribute>();
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<ApiExceptionFilter>();

            }).AddJsonOptions((options) =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonDateConverter());
            });
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddUEditorService();
            //ע��������
            services.AddCorsPolicy(Configuration);

            //ע��jwt��֤
            services.AddJwtService(Configuration);

            //swagger����
            services.AddSwaggerGen(options =>
            {
                //��xmlע������xml�ĵ�
                options.SwaggerDoc("v1", new OpenApiInfo
                {

                    Title = "����ʵ���ƺ�̨�ӿ��ĵ�",
                    Version = "v1",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "",
                        Url = new Uri("http://wwa/cs.com")
                    },
                    License = new OpenApiLicense { Name = "" }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "OpenApi.xml");
                options.IncludeXmlComments(xmlPath, true);
                //var entityXmlPath = Path.Combine(basePath, "DtoHelp.xml");
                //options.IncludeXmlComments(entityXmlPath);
                //options.IncludeXmlComments("../doc/CloudWeb.OpenApi/OpenApi.xml");

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            //����ע��
            app.UseStaticHostEnviroment();
            //����Ĭ��Ŀ¼
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath)),
            //});
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "upload")),
                RequestPath = "/upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });

            app.UseRouting();
            //���������м��
            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);

            app.UseAuthentication();

            //��Ȩ�м��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger����
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudWeb OpenApi service"));
        }
    }
}