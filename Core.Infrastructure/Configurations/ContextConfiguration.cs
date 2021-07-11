using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class ContextConfiguration
    {
        public static void ConfigureContext(this IServiceCollection services, string dbType, string connectionString)
        {
            switch (dbType)
            {
                case "MsSql":
                    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
                    break;
                case "PgSql":
                    services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));
                    break;
                default:
                    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
                    break;
            }

            services.AddScoped<UnitOfWork<ApplicationContext>>();
        }
    }
}