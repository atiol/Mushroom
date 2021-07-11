using AutoMapper;
using Core.Common.Extensions;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Model.ViewModels;

namespace Core.Infrastructure
{
    internal sealed class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserDtoModel>()
                .ForMember(dto => dto.UserRoles, opt => opt.MapFrom(entity => entity.UserRoles))
                .ForMember(dto => dto.UserRefreshTokens, opt => opt.MapFrom(entity => entity.UserRefreshTokens))
                .ReverseMap();
            CreateMap<UserDtoModel, UserViewModel>()
                .ForMember(view => view.UserRoles, opt => opt.MapFrom(dto => dto.UserRoles));
            CreateMap<UserViewModel, UserDtoModel>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(view => view.Name.ToBeautify()))
                .ForMember(dto => dto.SurName, opt => opt.MapFrom(view => view.SurName.ToBeautify()))
                .ForMember(dto => dto.NormalizedEmail, opt => opt.MapFrom(view => view.Email.ToNormalize()))
                .ForMember(dto => dto.NormalizedUserName, opt => opt.MapFrom(view => view.UserName.ToNormalize()))
                .ForMember(dto => dto.UserRoles, opt => opt.MapFrom(view => view.UserRoles));

            CreateMap<UserRolesEntity, UserRolesDtoModel>().ReverseMap();
            CreateMap<UserRolesDtoModel, UserRolesViewModel>().ReverseMap();

            CreateMap<UserRefreshTokenEntity, UserRefreshTokenDtoModel>().ReverseMap();
            CreateMap<UserRefreshTokenDtoModel, UserRefreshTokenViewModel>().ReverseMap();

            CreateMap<RoleEntity, RoleDtoModel>()
                .ForMember(dto => dto.UserRoles, opt => opt.MapFrom(entity => entity.UserRoles))
                .ForMember(dto => dto.RoleAcls, opt => opt.MapFrom(entity => entity.RoleAcls))
                .ReverseMap();
            CreateMap<RoleDtoModel, RoleViewModel>()
                .ForMember(view => view.RoleAcls, opt => opt.MapFrom(dto => dto.RoleAcls));
            CreateMap<RoleViewModel, RoleDtoModel>()
                .ForMember(dto => dto.Name, opt => opt.MapFrom(view => view.Name.ToBeautify()))
                .ForMember(dto => dto.NormalizedName, opt => opt.MapFrom(view => view.Name.ToNormalize()))
                .ForMember(dto => dto.RoleAcls, opt => opt.MapFrom(view => view.RoleAcls));

            CreateMap<RoleAclsEntity, RoleAclsDtoModel>().ReverseMap();
            CreateMap<RoleAclsDtoModel, RoleAclsViewModel>().ReverseMap();
        }
    }
}