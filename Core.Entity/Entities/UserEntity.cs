using System.Collections.Generic;
using Core.Entity.Abstracts;

namespace Core.Entity.Entities
{
    public sealed class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        
        public ICollection<UserRolesEntity> UserRoles { get; set; }
        public ICollection<UserRefreshTokenEntity> UserRefreshTokens { get; set; }
    }
}