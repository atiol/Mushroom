using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Api.Abstracts;
using Core.Common.Helpers;
using Core.Common.Models;
using Core.Model.DtoModels;
using Core.Model.ViewModels;
using Core.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    public sealed class HomeController : BaseController
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        
        public HomeController(IServiceProvider serviceProvider, UserService userService, RoleService roleService) : base( serviceProvider)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var rViewModel = new RoleViewModel("test", new List<RoleAclsViewModel>
            {
                new RoleAclsViewModel("users.module", 0),
                new RoleAclsViewModel("users.module.add", 0)
            });

            var rDtoModel = GetMapper().Map<RoleDtoModel>(rViewModel);
            
            var r = await _roleService.Save(rDtoModel);
            
            var uViewModel = new UserViewModel("özhan", "uslu", "ozhan.uslu", "usluozhan33@gmail.com", CryptoHelper.Encrypt("1234", GetConfigurationValue("Jwt:Secret")), new List<UserRolesViewModel>
            {
                new UserRolesViewModel(0, r.Id)
            });

            var uDtoModel = GetMapper().Map<UserDtoModel>(uViewModel);
            
            await _userService.Save(uDtoModel);
            
            return Ok(new ApiResponse<bool>(ResponseMessageCommonService.Success, GetLanguage(), true));
        }
    }
}