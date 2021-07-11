using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Core.Service.Services
{
    public sealed class RoleService : BaseService<ApplicationContext, RoleEntity, RoleDtoModel>
    {
        public RoleService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<IEnumerable<string>> GetRolesAclKeys(IEnumerable<long> roleIds)
        {
            var roles = await GetAll(null, rol => roleIds.Contains(rol.Id), null,
                rol => rol.Include(r => r.RoleAcls));

            var aclList = new List<string>();

            foreach (var roleViewModel in roles)
            {
                foreach (var roleAclsViewModel in roleViewModel.RoleAcls)
                {
                    if (aclList.Contains(roleAclsViewModel.AclKey))
                    {
                        continue;
                    }

                    aclList.Add(roleAclsViewModel.AclKey);
                }
            }

            return aclList;
        }
    }
}