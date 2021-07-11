using Core.Model.Abstracts;
using FluentValidation;

namespace Core.Model.ViewModels
{
    public sealed class UserRolesViewModel : BaseViewModel
    {
        public long RoleId { get; set; }
        public long UserId { get; set; }
        
        public UserRolesViewModel(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRolesViewModel()
        {
        }
    }
    
    public sealed class UserRolesViewModelValidator : AbstractValidator<UserRolesViewModel>
    {
        public UserRolesViewModelValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty();
        }
    }
}