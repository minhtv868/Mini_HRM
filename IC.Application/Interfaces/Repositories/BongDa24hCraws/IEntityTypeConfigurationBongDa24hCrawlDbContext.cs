using Microsoft.EntityFrameworkCore;

namespace IC.Application.Interfaces.Repositories.BongDa24hCrawls
{
    public interface IEntityTypeConfigurationBongDa24hCrawlDbContext<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }
}
