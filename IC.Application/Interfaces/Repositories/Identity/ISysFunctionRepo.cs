using IC.Domain.Entities.Identity;

namespace IC.Application.Interfaces.Repositories.Identity
{
	public interface ISysFunctionRepo
    {
        Task UpdateTreeOrder();
        Task<List<SysFunction>> GetMenuByUserId(int userId);
        Task<List<SysFunction>> GetMenuByUserName(string userName);
		Task<bool> UserHasPermissionOnUrl(string userName, string url, bool allowParentUrl = true);
		Task<bool> UserHasPermissionOnUrl(int userId, string url, bool allowParentUrl = true);
	}
}
