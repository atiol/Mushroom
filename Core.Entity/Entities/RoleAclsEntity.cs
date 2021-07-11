using Core.Entity.Abstracts;

namespace Core.Entity.Entities
{
    public sealed class RoleAclsEntity : BaseEntity
    {
        public string AclKey { get; set; }
        public long RoleId { get; set; }
        
        public RoleEntity Role { get; set; }
    }
}