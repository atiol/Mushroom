using System;
using System.Threading.Tasks;
using Core.Api.Abstracts;
using Core.Common.Models;
using Core.Model.RequestModels;
using Core.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers
{
    public sealed class AuthController : BaseController
    {
        private readonly UserService _userService;
        private readonly UserRefreshTokenService _userRefreshTokenService;

        public AuthController(IServiceProvider serviceProvider, UserService userService,
            UserRefreshTokenService userRefreshTokenService) : base(serviceProvider)
        {
            _userService = userService;
            _userRefreshTokenService = userRefreshTokenService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] string base64Credential)
        {
            var token = await _userService.Authenticate(base64Credential);

            return string.IsNullOrEmpty(token)
                ? Ok(new ApiResponse(ResponseMessageCommonService.LoginFail, GetLanguage()))
                : Ok(new ApiResponse<string>(ResponseMessageCommonService.Success, GetLanguage(), token));
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestModel refreshTokenRequestModel)
        {
            var token = await _userRefreshTokenService.Refresh(refreshTokenRequestModel.AccessToken,
                refreshTokenRequestModel.RefreshToken);

            return string.IsNullOrEmpty(token)
                ? Ok(new ApiResponse(ResponseMessageCommonService.RefreshFail, GetLanguage()))
                : Ok(new ApiResponse<string>(ResponseMessageCommonService.Success, GetLanguage(), token));
        }
    }
}