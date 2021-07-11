using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class HangFireConfiguration
    {
        public static void ConfigureHangFireService(this IServiceCollection services, string dbType,
            string connectionString)
        {
            switch (dbType)
            {
                case "MsSql":
                    services.AddHangfire(options => options.UseSqlServerStorage(connectionString));
                    break;
                case "PgSql":
                    services.AddHangfire(options => options.UsePostgreSqlStorage(connectionString));
                    break;
                default:
                    services.AddHangfire(options => options.UseSqlServerStorage(connectionString));
                    break;
            }
        }

        public static void ConfigureHangFireApp(this IApplicationBuilder app, bool enableDashboard,
            string dashboardPath, bool dashboardReadOnly, string appDomain)
        {
            if (enableDashboard)
            {
                app.UseHangfireDashboard(dashboardPath, new DashboardOptions
                {
                    IsReadOnlyFunc = context => dashboardReadOnly,
                    AppPath = appDomain,
                    DisplayStorageConnectionString = false
                });
            }

            app.UseHangfireServer();
        }
    }
}