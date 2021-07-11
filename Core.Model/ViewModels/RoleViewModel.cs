using System.Collections.Generic;
using Core.Model.Abstracts;
using FluentValidation;

namespace Core.Model.ViewModels
{
    public sealed class RoleViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public ICollection<RoleAclsViewModel> RoleAcls { get; set; }
        
        public RoleViewModel(string name, ICollection<RoleAclsViewModel> roleAcls)
        {
            Name = name;
            RoleAcls = roleAcls;
        }

        public RoleViewModel()
        {
        }
    }
    
    public sealed class RoleViewModelValidator : AbstractValidator<RoleViewModel>
    {
        public RoleViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.RoleAcls).NotEmpty();
        }
    }
}