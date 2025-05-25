namespace IC.Application.Interfaces
{
    public interface IAuditableService
    {
        Task UpdateAuditableInfoAsync<T>(List<T> data) where T : IAuditable;
    }
}
