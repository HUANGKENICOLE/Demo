using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Demo_One
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // IServiceCollection服务配置接口，用来配置常用服务及依赖注入
        public void ConfigureServices(IServiceCollection services)
        {
            //注册 DbContext 注入到ASP.NET Core运行时。
            // 要想在 ASP.NET Core 中使用，要将PatientDbContext注入到ServiceCollection容器中
            // 这里用pgsql作为底层存储，安装途径：先去EF官方文档上找到pqsql对应的NuGet程序包再进行安装
            //是Npgsql.EntityFrameworkCore.PostgreSQL，而不是Npgsql
            // 记住
            services.AddDbContext<PatientDbContext>(opt => opt.UseNpgsql(Configuration["ConnectionStrings:aliyun_pgsql"]));

           
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Demo_One", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo_One v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // config.MapHttpAttributeRoutes();
            // app.Routes.MapHttpRoute(
            //     name: "WebApiTest",
            //     routeTemplate: "api/{controller}/{action}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );
            
            // app.UseMvc(routes=>)
            // {
            //     RouteData.RouteDataSnapshot.MapRoute(
            //         name:"default",
            //         template:"{controller=Home}/{action=Index}/{id?}";
            //         )
            // }

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
        }
    }
}