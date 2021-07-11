using Core.Model.RequestModels;
using Core.Model.ViewModels;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Configurations
{
    public static class ModelValidatorConfiguration
    {
        public static void ConfigureModelValidators(this IServiceCollection services)
        {
            services.AddSingleton<IValidator<RefreshTokenRequestModel>, RefreshTokenRequestModelValidator>();
            
            services.AddSingleton<IValidator<UserViewModel>, UserViewModelValidator>();
            services.AddSingleton<IValidator<RoleViewModel>, RoleViewModelValidator>();
            services.AddSingleton<IValidator<UserRolesViewModel>, UserRolesViewModelValidator>();
            services.AddSingleton<IValidator<RoleAclsViewModel>, RoleAclsViewModelValidator>();
        }
    }
}