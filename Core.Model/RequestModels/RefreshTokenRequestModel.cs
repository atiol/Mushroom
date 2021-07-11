using FluentValidation;

namespace Core.Model.RequestModels
{
    public sealed class RefreshTokenRequestModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        
        public RefreshTokenRequestModel()
        {
        }
    }
    
    public sealed class RefreshTokenRequestModelValidator : AbstractValidator<RefreshTokenRequestModel>
    {
        public RefreshTokenRequestModelValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}