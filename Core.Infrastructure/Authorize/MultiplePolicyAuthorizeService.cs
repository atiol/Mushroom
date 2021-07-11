using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Service.Services;

namespace Core.Infrastructure.Authorize
{
    internal sealed class MultiplePolicyAuthorizeService
    {
        private readonly UserService _userService;
        private IEnumerable<string> _userAcls;

        public MultiplePolicyAuthorizeService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<string>> GetUserAcls(long userId)
        {
            if (_userAcls != null && _userAcls.Any())
            {
                return _userAcls;
            }

            _userAcls = await _userService.GetUserRolesAcls(userId);

            return _userAcls;
        }
    }
}