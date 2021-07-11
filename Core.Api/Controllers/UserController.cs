using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Api.Abstracts;
using Core.Common.Constants;
using Core.Common.Models;
using Core.Infrastructure.Authorize;
using Core.Model.DtoModels;
using Core.Model.ViewModels;
using Core.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    [MultipleAclAuthorize(ConstantAcls.UserModuleAclKey)]
    public sealed class UserController : BaseController
    {
        private readonly UserService _userService;

        public UserController(IServiceProvider serviceProvider, UserService userService) : base(serviceProvider)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpGet("GetUserAcls/{userId:long}")]
        public async Task<IActionResult> GetUserAcls(long userId)
        {
            var userAcls = await _userService.GetUserRolesAcls(userId);

            return Ok(new ApiResponse<IEnumerable<string>>(ResponseMessageCommonService.Success, GetLanguage(), userAcls));
        }

        [MultipleAclAuthorize(ConstantAcls.UserModuleAddAclKey)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserViewModel userViewModel)
        {
            var dtoModel = GetMapper().Map<UserDtoModel>(userViewModel);
            
            return Ok(await _userService.SaveUser(dtoModel));
        }
    }
}