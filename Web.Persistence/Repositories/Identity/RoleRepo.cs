using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Domain.Entities.Identity;
using Web.Persistence.Contexts;

namespace Web.Persistence.Repositories.Identity
{
    public class RoleRepo : IRoleRepo
    {
        private readonly FinanceDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public RoleRepo(FinanceDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = await _currentUserService.GetRoles();

            bool isSuperAdminRole = _currentUserService.IsSuperAdminRole(roles);

            if (isSuperAdminRole)
            {
                return await _dbContext.Roles.ToListAsync();

            }
            else
            {
                return await _dbContext.Roles.Where(x => x.Name != "Admin").ToListAsync();
            }
        }
    }
}