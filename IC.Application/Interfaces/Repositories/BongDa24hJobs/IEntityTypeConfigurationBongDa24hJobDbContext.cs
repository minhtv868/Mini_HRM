using Microsoft.EntityFrameworkCore;

namespace IC.Application.Interfaces.Repositories.BongDa24hJobs
{
    public interface IEntityTypeConfigurationBongDa24hJobDbContext<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }
}
