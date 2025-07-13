using Web.Application.Interfaces.Repositories.Identity;
using Web.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.Persistence.Repositories.Identity
{
    public class UserRoleRepo : IUserRoleRepo
    {
        private readonly FinanceDbContext _dbContext;

		public DbSet<IdentityUserRole<int>> DbSet => _dbContext.UserRoles;

		public UserRoleRepo(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<IdentityUserRole<int>>> GetByUserIdAsync(int id)
        {
            return await _dbContext.UserRoles.Where(x => x.UserId == id).ToListAsync();
        }

		public async Task<List<IdentityUserRole<int>>> GetByUserIdsListAsync(List<int> userIds)
		{
			return await _dbContext.UserRoles.Where(x => userIds.Any(u => u == x.UserId)).ToListAsync();
		}

		public async Task<List<IdentityUserRole<int>>> GetAllAsync()
        {
            return await _dbContext.UserRoles.ToListAsync();
        }
	}
}