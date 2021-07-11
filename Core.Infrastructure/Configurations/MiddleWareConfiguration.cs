using Core.Infrastructure.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace Core.Infrastructure.Configurations
{
    public static class MiddleWareConfiguration
    {
        public static void ConfigureMiddleWareApp(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleWare>();
        }
    }
}