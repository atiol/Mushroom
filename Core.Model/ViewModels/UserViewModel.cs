using System.Collections.Generic;
using Core.Model.Abstracts;
using FluentValidation;

namespace Core.Model.ViewModels
{
    public sealed class UserViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<UserRolesViewModel> UserRoles { get; set; }

        public UserViewModel(string name, string surName, string userName, string email, string password,
            ICollection<UserRolesViewModel> userRoles)
        {
            Name = name;
            SurName = surName;
            UserName = userName;
            Email = email;
            Password = password;
            UserRoles = userRoles;
        }
        
        public UserViewModel()
        {
        }
    }

    public sealed class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SurName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.UserRoles).NotEmpty();
        }
    }
}