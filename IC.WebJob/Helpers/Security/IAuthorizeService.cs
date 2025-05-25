namespace IC.WebJob.Helpers.Security
{
    public interface IAuthorizeService
    {
        bool HasPermission(int userId, string itemUrl);
        bool HasPermission(string userName, string itemUrl);
        bool HasPermission(string itemUrl, byte[] sessionData);
    }
}
