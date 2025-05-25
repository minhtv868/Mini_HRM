using IC.Application.Interfaces;
using IC.Application.Interfaces.Repositories.Identity;
using IC.Domain.Entities.Identity;
using IC.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IC.Persistence.Repositories.Identity
{
    public class RoleRepo : IRoleRepo
    {
        private readonly ICIdentityDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public RoleRepo(ICIdentityDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
			_currentUserService = currentUserService;
		}

        public async Task<List<Role>> GetAllAsync()
        {
			var roles = await _currentUserService.GetRoles();

			bool isSuperAdminRole = _currentUserService.IsSuperAdminRole(roles);

            if(isSuperAdminRole)
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