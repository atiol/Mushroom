using Core.Common.Services;
using Core.Infrastructure.Authorize;
using Core.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ResponseMessageCommonService>();

            services.AddScoped<MultiplePolicyAuthorizeService>();

            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserRolesService>();
            services.AddScoped<UserRefreshTokenService>();
            services.AddScoped<RoleAclsService>();
        }
    }
}