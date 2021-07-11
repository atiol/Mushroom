using Core.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace Core.Common.Helpers
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public static string GetUserName()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return "system";
            }
            
            var user = _httpContextAccessor.HttpContext.User;

            var userNameClaim = user?.FindFirst(ConstantClaims.UserName);

            if (userNameClaim == null) return "system";
                
            return !string.IsNullOrEmpty(userNameClaim.Value) ? userNameClaim.Value : "system";
        }
        
        public static long? GetUserId()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }
            
            var user = _httpContextAccessor.HttpContext.User;

            var userIdClaim = user?.FindFirst(ConstantClaims.Id);

            if (userIdClaim == null) return null;

            if (string.IsNullOrEmpty(userIdClaim.Value))
            {
                return null;
            }
                
            return long.Parse(userIdClaim.Value);
        }
    }
}