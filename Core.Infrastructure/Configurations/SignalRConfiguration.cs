using Core.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class SignalRConfiguration
    {
        public static void ConfigureSignalRService(this IServiceCollection services, bool enabledRedis,
            string redisConnectionString)
        {
            if (enabledRedis)
            {
                services.AddSignalR().AddStackExchangeRedis(redisConnectionString);
            }
            else
            {
                services.AddSignalR();
            }
        }

        public static void ConfigureSignalRApp(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => { endpoints.MapHub<ChatHub>("/chatHub"); });
        }
    }
}