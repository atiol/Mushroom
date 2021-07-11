using System;
using Core.Model.Abstracts;

namespace Core.Model.DtoModels
{
    public sealed class UserRefreshTokenDtoModel : BaseDtoModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }
}