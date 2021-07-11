using System.Collections.Generic;
using Core.Entity.Abstracts;

namespace Core.Entity.Entities
{
    public sealed class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        
        public ICollection<RoleAclsEntity> RoleAcls { get; set; }
        public ICollection<UserRolesEntity> UserRoles { get; set; }
    }
}