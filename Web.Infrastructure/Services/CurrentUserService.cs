using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Application.Features.IdentityFeatures.SysFunctions.Queries;
using Web.Application.Interfaces;
using Web.Domain.Entities.Identity;

namespace Web.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null) ? 0 : int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        public string FullName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);

        public async Task<IEnumerable<string>> GetRoles()
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            return roles;
        }

        public bool IsUserRole(IEnumerable<string> roles)
        {
            return !roles.Any(x => x == "Admin")
                && !roles.Any(x => x == "MOD")
                && roles.Any(x => x == "USER");
        }

        public bool IsModRole(IEnumerable<string> roles)
        {
            return roles.Any(x => x == "MOD") && !roles.Any(x => x == "Admin");
        }

        public bool IsAdminRole(IEnumerable<string> roles)
        {
            return roles.Any(x => x == "Admin" || x == "Super Admin");
        }

        public bool IsSuperAdminRole(IEnumerable<string> roles)
        {
            return roles.Any(x => x == "Super Admin");
        }

        public async Task<bool> HasPermission(string handler = "", bool allowByParent = false)
        {
            string path = _httpContextAccessor.HttpContext?.Request.Path;

            if (!string.IsNullOrWhiteSpace(handler))
            {
                path = $"{path}?handler={handler}";
            }

            var result = await _mediator.Send(new SysFunctionCheckPermissionByUserQuery(UserId, path, allowByParent));

            return result.Data;
        }

        public async Task<bool> HasPermissionWithSite(int siteId)
        {
            var siteAll = await _mediator.Send(new SiteGetAllByUserQuery());
            var site = siteAll.Where(x => x.SiteId == siteId).FirstOrDefault();
            return site != null;
        }
    }
}
