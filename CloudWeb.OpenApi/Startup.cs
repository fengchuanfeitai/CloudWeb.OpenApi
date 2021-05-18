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

namespace CloudWeb.OpenApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddLogging(logger => { logger.AddConsole(); });

            //����ע��
            services.AddScoped<IUserService, UserService>();
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
                var entityXmlPath = Path.Combine(basePath, "DtoHelp.xml");
                options.IncludeXmlComments(entityXmlPath);
                //options.IncludeXmlComments("../doc/CloudWeb.OpenApi/OpenApi.xml");

                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                //options.AddSecurityRequirement(security);//���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������Ҫһ�£�������Bearer��
                //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) �����ṹ: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",//jwtĬ�ϵĲ�������
                //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                //    Type = "apiKey"
                //});
            });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin()//�����κ�Դ
                    .AllowAnyMethod()//�����κη�ʽ
                    .AllowAnyHeader()//�����κ�ͷ
                    .AllowCredentials();//����cookie
                });
                c.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("http://localhost:8083")
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithHeaders("authorization");
                });
            });

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