using System.Collections.Generic;
using Core.Model.Abstracts;

namespace Core.Model.DtoModels
{
    public sealed class RoleDtoModel : BaseDtoModel
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public ICollection<RoleAclsDtoModel> RoleAcls { get; set; }
        public ICollection<UserRolesDtoModel> UserRoles { get; set; }
    }
}