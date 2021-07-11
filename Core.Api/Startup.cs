using Core.Common.Helpers;
using Core.Infrastructure.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureContext(_configuration["Db:Type"], _configuration["Db:ConnectionString"]);

            if (bool.Parse(_configuration["HangFire:Enable"]))
            {
                services.ConfigureHangFireService(_configuration["Db:Type"], _configuration["Db:ConnectionString"]);
            }

            if (bool.Parse(_configuration["SignalR:Enable"]))
            {
                services.ConfigureSignalRService(bool.Parse(_configuration["SignalR:Redis:Enable"]),
                    _configuration["SignalR:Redis:ConnectionString"]);
            }

            services.ConfigureAutoMapper();

            services.ConfigureServices();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddCors();

            services.AddResponseCompression();

            services.ConfigureModelValidators();

            services.AddControllers().AddFluentValidation();

            services.ConfigureJwt(_configuration["Jwt:Secret"]);

            if (bool.Parse(_configuration["EnableSwagger"]))
            {
                services.ConfigureSwaggerService();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSerilogRequestLogging();
            }

            if (bool.Parse(_configuration["HangFire:Enable"]))
            {
                app.ConfigureHangFireApp(bool.Parse(_configuration["HangFire:EnableDashboard"]),
                    _configuration["HangFire:Path"], bool.Parse(_configuration["HangFire:ReadOnly"]),
                    _configuration["Domain"]);
            }

            app.UseCors();

            app.UseResponseCompression();

            app.UseForwardedHeaders();

            app.ConfigureMiddleWareApp();

            if (bool.Parse(_configuration["EnableSwagger"]))
            {
                app.ConfigureSwaggerApp();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            HttpContextHelper.SetHttpContextAccessor(app.ApplicationServices.GetService<IHttpContextAccessor>());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("TimeData Core.Net ..."); });
                endpoints.MapControllers();
            });

            if (bool.Parse(_configuration["SignalR:Enable"]))
            {
                app.ConfigureSignalRApp();
            }
        }
    }
}