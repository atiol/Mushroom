using System;
using Core.Data;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Service.Abstracts;

namespace Core.Service.Services
{
    public sealed class UserRolesService : BaseService<ApplicationContext, UserRolesEntity, UserRolesDtoModel>
    {
        public UserRolesService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}