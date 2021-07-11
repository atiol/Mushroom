using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Constants;
using Core.Common.Extensions;
using Core.Common.Helpers;
using Core.Common.Models;
using Core.Data;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Model.ViewModels;
using Core.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Core.Service.Services
{
    public sealed class UserService : BaseService<ApplicationContext, UserEntity, UserDtoModel>
    {
        private readonly RoleService _roleService;
        private readonly UserRefreshTokenService _userRefreshTokenService;

        public UserService(IServiceProvider serviceProvider, RoleService roleService,
            UserRefreshTokenService userRefreshTokenService) : base(serviceProvider)
        {
            _roleService = roleService;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<string> Authenticate(string base64Credential)
        {
            if (string.IsNullOrEmpty(base64Credential) || !base64Credential.StartsWith("Basic "))
            {
                return null;
            }

            var encodedUsernamePassword =
                base64Credential.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1].Trim();

            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var userName = decodedUsernamePassword.Split(':', 2)[0];

            var user = await GetRepository().GetFirstOrDefaultAsQueryable(null, usr => usr.UserName == userName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var password = decodedUsernamePassword.Split(':', 2)[1];
            var secret = GetConfigurationValue("Jwt:Secret");

            if (CryptoHelper.Decrypt(user.Password, secret) != password)
            {
                return null;
            }

            var key = Encoding.ASCII.GetBytes(secret);
            var refreshToken = CryptoHelper.GenerateRefreshToken();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ConstantClaims.Id, user.Id.ToString()),
                    new Claim(ConstantClaims.Name, user.Name),
                    new Claim(ConstantClaims.SurName, user.SurName),
                    new Claim(ConstantClaims.Email, user.Email),
                    new Claim(ConstantClaims.UserName, user.UserName),
                    new Claim(ConstantClaims.RefreshToken, refreshToken)
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(GetConfigurationValue("Jwt:ExpireDay"))),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userRefreshTokenViewModel = new UserRefreshTokenViewModel(user.Id, refreshToken,
                DateTime.Now.AddDays(Convert.ToInt32(GetConfigurationValue("Jwt:RefreshTokenExpireDay"))));

            var userRefreshTokenDtoModel = GetMapper().Map<UserRefreshTokenDtoModel>(userRefreshTokenViewModel);

            await _userRefreshTokenService.Save(userRefreshTokenDtoModel);

            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<string>> GetUserRolesAcls(long userId)
        {
            var user = await GetRepository().GetFirstOrDefaultAsQueryable(null, usr => usr.Id == userId,
                    usr => usr.Include(r => r.UserRoles))
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var roleIds = user.UserRoles.Select(r => r.RoleId).ToList();

            return await _roleService.GetRolesAclKeys(roleIds);
        }

        public async Task<ApiResponse> SaveUser(UserDtoModel userDtoModel)
        {
            var existUser = await GetExist(u => u.NormalizedUserName == userDtoModel.UserName.ToNormalize());

            if (existUser)
            {
                return new ApiResponse(ResponseMessageCommonService.UserNameExist, GetLanguage());
            }

            userDtoModel.Password = CryptoHelper.Encrypt(userDtoModel.Password, GetConfigurationValue("Jwt:Secret"));
            var entity = GetMapper().Map<UserEntity>(userDtoModel);

            await GetRepository().Add(entity);
            await GetUnitOfWork().Commit();

            return new ApiResponse(ResponseMessageCommonService.Success, GetLanguage());
        }
    }
}