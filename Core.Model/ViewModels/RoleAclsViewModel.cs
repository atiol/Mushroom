using Core.Model.Abstracts;
using FluentValidation;

namespace Core.Model.ViewModels
{
    public sealed class RoleAclsViewModel : BaseViewModel
    {
        public string AclKey { get; set; }
        public long RoleId { get; set; }

        public RoleAclsViewModel(string aclKey, long roleId)
        {
            AclKey = aclKey;
            RoleId = roleId;
        }
    }

    public sealed class RoleAclsViewModelValidator : AbstractValidator<RoleAclsViewModel>
    {
        public RoleAclsViewModelValidator()
        {
            RuleFor(x => x.AclKey).NotEmpty();
        }
    }
}