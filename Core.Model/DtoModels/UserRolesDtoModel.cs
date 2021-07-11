using Core.Model.Abstracts;

namespace Core.Model.DtoModels
{
    public sealed class UserRolesDtoModel : BaseDtoModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}