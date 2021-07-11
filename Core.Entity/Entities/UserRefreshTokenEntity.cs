using System;
using Core.Entity.Abstracts;

namespace Core.Entity.Entities
{
    public sealed class UserRefreshTokenEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
        
        public UserEntity User { get; set; }
    }
}