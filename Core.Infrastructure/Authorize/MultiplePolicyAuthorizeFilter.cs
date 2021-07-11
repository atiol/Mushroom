using System.Linq;
using System.Threading.Tasks;
using Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Infrastructure.Authorize
{
    internal sealed class MultipleAclAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private string Policies { get; }
        private bool IsAnd { get; }

        private readonly MultiplePolicyAuthorizeService _multiplePolicyAuthorizeService;

        public MultipleAclAuthorizeFilter(string policies, bool isAnd,
            MultiplePolicyAuthorizeService multiplePolicyAuthorizeService)
        {
            Policies = policies;
            IsAnd = isAnd;
            _multiplePolicyAuthorizeService = multiplePolicyAuthorizeService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = HttpContextHelper.GetUserId();

            if (userId == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var routeAcls = Policies.Split(';');
            var userAcls = await _multiplePolicyAuthorizeService.GetUserAcls(userId.Value);

            if (IsAnd)
            {
                if (routeAcls.All(routeAcl => userAcls.Contains(routeAcl)))
                {
                    return;
                }
            }
            else
            {
                if (routeAcls.Any(routeAcl => userAcls.Contains(routeAcl)))
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}