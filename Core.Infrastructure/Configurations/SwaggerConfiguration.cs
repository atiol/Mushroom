using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Core.Infrastructure.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("Core.Net", new OpenApiInfo {Title = "Core.Net Api Doc From Swagger"});

                s.AddFluentValidationRules();

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey 
                });
                
                s.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                        new OpenApiSecurityScheme 
                        { 
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" 
                            } 
                        },
                        new string[] { } 
                    } 
                });
            });
        }

        public static void ConfigureSwaggerApp(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/Core.Net/swagger.json", "Core.Net Swagger"); });
        }
    }
}