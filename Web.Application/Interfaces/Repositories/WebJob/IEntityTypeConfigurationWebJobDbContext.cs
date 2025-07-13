using Microsoft.EntityFrameworkCore;

namespace Web.Application.Interfaces.Repositories.BongDa24hCrawls
{
    public interface IEntityTypeConfigurationWebJobDbContext<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }
}
