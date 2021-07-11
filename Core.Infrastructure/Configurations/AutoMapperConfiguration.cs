using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}