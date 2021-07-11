using System.Collections.Generic;
using Core.Model.Abstracts;

namespace Core.Model.DtoModels
{
    public sealed class UserDtoModel : BaseDtoModel
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
        public ICollection<UserRolesDtoModel> UserRoles { get; set; }
        public ICollection<UserRefreshTokenDtoModel> UserRefreshTokens { get; set; }
    }
}