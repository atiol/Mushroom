using Core.Model.Abstracts;

namespace Core.Model.DtoModels
{
    public sealed class RoleAclsDtoModel : BaseDtoModel
    {
        public string AclKey { get; set; }
        public long RoleId { get; set; }
    }
}