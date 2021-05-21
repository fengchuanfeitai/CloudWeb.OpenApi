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

        //添加autofac的DI配置容器
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册IUserService和UserService接口及对应的实现类
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ColumnService>().As<IColumnService>().InstancePerLifetimeScope();
            builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();

            //注册aop拦截器 
            //将业务层程序集名称传了进去，给业务层接口和实现做了注册，也给业务层各方法开启了代理
            builder.AddAopService(ServiceExtensions.GetAssemblyName());
        }  //依赖注入

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //注册统一结果返回，异常过滤器
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<ApiExceptionFilter>();
            });


            //注册跨域策略
            services.AddCorsPolicy(Configuration);

            //注册jwt验证
            services.AddJwtService(Configuration);

            //swagger依赖
            services.AddSwaggerGen(options =>
            {
                //从xml注释生成xml文档
                options.SwaggerDoc("v1", new OpenApiInfo
                {

                    Title = "仿真实验云后台接口文档",
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

            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowAnyOrigin", policy =>
            //    {
            //        policy.AllowAnyOrigin()//允许任何源
            //        .AllowAnyMethod()//允许任何方式
            //        .AllowAnyHeader()//允许任何头
            //        .AllowCredentials();//允许cookie
            //    });
            //    c.AddPolicy("AllowSpecificOrigin", policy =>
            //    {
            //        policy.WithOrigins("http://localhost:8083")
            //        .WithMethods("GET", "POST", "PUT", "DELETE")
            //        .WithHeaders("authorization");
            //    });
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //开启跨域中间件
            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);

            app.UseAuthentication();
            //授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //swagger配置
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudWeb OpenApi service"));
        }
    }
}