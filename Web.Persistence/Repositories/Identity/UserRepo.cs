using Web.Application.Interfaces.Repositories.Identity;
using Web.Domain.Entities.Identity;
using Web.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Web.Persistence.Repositories.Identity
{
	public class UserRepo : IUserRepo
    {
        private readonly FinanceDbContext _dbContext;

        public UserRepo(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _dbContext.Users.Where(x => x.UserName == userName).SingleOrDefaultAsync();
        }

        public async Task UpdateLastTimeLogin(string userName)
        {
            var user = await GetByUserNameAsync(userName);
            if (user != null)
            {
                user.LastTimeLogin = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateLastTimeChangePass(string userName)
        {
            var user = await GetByUserNameAsync(userName);
            if (user != null)
            {
                user.LastTimeChangePass = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }

		public async Task<List<User>> GetListUser(List<int> userIds)
		{
            List<User> usersList = await _dbContext.Users
                .Where(x => userIds.Any(u => u == x.Id))
                .Select(x => new User() { Id = x.Id, UserName = x.UserName })
                .ToListAsync();

            return usersList;
		}
        public async Task<List<User>> GetAllUser()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        } 

		public async Task<User> GetByIdAsync(int id)
		{
			return await _dbContext.Users.Where(x => x.Id == id).SingleOrDefaultAsync();
		}
	}
}