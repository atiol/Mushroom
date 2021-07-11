using System;
using Core.Data;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Service.Abstracts;

namespace Core.Service.Services
{
    public sealed class RoleAclsService : BaseService<ApplicationContext, RoleAclsEntity, RoleAclsDtoModel>
    {
        public RoleAclsService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}