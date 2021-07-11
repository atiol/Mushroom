using System;
using Core.Model.Abstracts;

namespace Core.Model.ViewModels
{
    public sealed class UserRefreshTokenViewModel : BaseViewModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
        
        public UserRefreshTokenViewModel(long userId, string token, DateTime expire)
        {
            UserId = userId;
            Token = token;
            Expire = expire;
        }
    }
}