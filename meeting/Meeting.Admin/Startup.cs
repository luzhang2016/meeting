using Meeting.Admin.core;
using Meeting.Admin.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace Meeting.Admin
{
  /// <summary>
  /// 开始类
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="env"></param>
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    /// <summary>
    /// 配置
    /// </summary>
    public IConfigurationRoot Configuration { get; }

    /// <summary>
    /// 添加服务到容器
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
      services.AddMvc();
      services.AddSwaggerGen();
      services.ConfigureSwaggerGen(o => {
        o.SingleApiVersion(new Swashbuckle.Swagger.Model.Info { Version = "v1", Title = "Winsoft APIs" });

        o.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Meeting.Admin.XML"));
        o.DescribeAllEnumsAsStrings();
      });

      services.AddOptions();
      services.Configure<AppSettings>(Configuration.GetSection("Settings"));

      services.AddSingleton<MeetingDataService>();
      services.AddSingleton<FileDataService>();
      services.AddSingleton<TempDataService>();
      services.AddSingleton<TemplateDataService>();
      services.AddSingleton<GroupDataService>();
      services.AddSingleton<SystemLogDataService>();
    }

    /// <summary>
    /// 配置路由规则
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseSwagger();
      app.UseSwaggerUi();

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
            ForwardedHeaders.XForwardedProto
      });

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
