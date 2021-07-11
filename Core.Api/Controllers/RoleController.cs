using System;
using Core.Api.Abstracts;

namespace Core.Api.Controllers
{
    public sealed class RoleController : BaseController
    {
        public RoleController(IServiceProvider serviceProvider) : base( serviceProvider)
        {
        }
    }
}