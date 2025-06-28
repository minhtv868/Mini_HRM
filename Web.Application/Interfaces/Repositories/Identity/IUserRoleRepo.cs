using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Interfaces.Repositories.Identity
{
	public interface IUserRoleRepo
    {
		DbSet<IdentityUserRole<int>> DbSet { get; }
		Task<List<IdentityUserRole<int>>> GetByUserIdAsync(int id);
		Task<List<IdentityUserRole<int>>> GetByUserIdsListAsync(List<int> userIds);
		Task<List<IdentityUserRole<int>>> GetAllAsync();
    }
}
