using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Constants;
using Core.Common.Helpers;
using Core.Data;
using Core.Entity.Entities;
using Core.Model.DtoModels;
using Core.Service.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Core.Service.Services
{
    public sealed class UserRefreshTokenService : BaseService<ApplicationContext, UserRefreshTokenEntity, UserRefreshTokenDtoModel>
    {
        public UserRefreshTokenService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<string> Refresh(string accessToken, string refreshToken)
        {
            await SoftDeleteByExpression(t => DateTime.Now > t.Expire);

            var userId = GetUserIdFromAccessToken(accessToken);

            if (userId == null)
            {
                return null;
            }

            var userRefreshTokenDtoModel = await GetOneOrNull(null,
                t => t.UserId == userId && t.Token == refreshToken);

            if (userRefreshTokenDtoModel == null)
            {
                return null;
            }

            var user = await GetUnitOfWork().GetRepository<UserEntity>().GetFirstOrDefaultAsQueryable(userId.Value)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var newRefreshToken = CryptoHelper.GenerateRefreshToken();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ConstantClaims.Id, user.Id.ToString()),
                    new Claim(ConstantClaims.Name, user.Name),
                    new Claim(ConstantClaims.SurName, user.SurName),
                    new Claim(ConstantClaims.Email, user.Email),
                    new Claim(ConstantClaims.UserName, user.UserName),
                    new Claim(ConstantClaims.RefreshToken, newRefreshToken)
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(GetConfigurationValue("Jwt:ExpireDay"))),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetConfigurationValue("Jwt:Secret"))),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            userRefreshTokenDtoModel.Token = newRefreshToken;
            userRefreshTokenDtoModel.Expire =
                DateTime.Now.AddDays(Convert.ToInt32(GetConfigurationValue("Jwt:RefreshTokenExpireDay")));

            await UpdateById(userRefreshTokenDtoModel.Id, userRefreshTokenDtoModel);

            return tokenHandler.WriteToken(token);
        }

        private long? GetUserIdFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetConfigurationValue("Jwt:Secret")))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            var userId = principal.FindFirst(ConstantClaims.Id)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return long.Parse(userId);
        }
    }
}