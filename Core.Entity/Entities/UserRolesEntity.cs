using Core.Entity.Abstracts;

namespace Core.Entity.Entities
{
    public sealed class UserRolesEntity : BaseEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
    }
}