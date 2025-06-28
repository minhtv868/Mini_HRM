namespace Web.Application.Interfaces
{
	public interface ICurrentUserService
	{
		int UserId { get; }
		string UserName { get; }
		string FullName { get; }
		Task<IEnumerable<string>> GetRoles();
		bool IsSuperAdminRole(IEnumerable<string> roles);
		bool IsAdminRole(IEnumerable<string> roles);
        bool IsModRole(IEnumerable<string> roles);
		bool IsUserRole(IEnumerable<string> roles);
        Task<bool> HasPermissionWithSite(int siteId);
        Task<bool> HasPermission(string handler = "", bool allowByParent = false);
	}
}
