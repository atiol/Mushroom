using Microsoft.AspNetCore.Mvc;

namespace Core.Infrastructure.Authorize
{
    public sealed class MultipleAclAuthorizeAttribute : TypeFilterAttribute
    {
        public MultipleAclAuthorizeAttribute(string policies, bool isAnd = false) : base(typeof(MultipleAclAuthorizeFilter))
        {
            Arguments = new object[] { policies, isAnd };
        }
    }
}