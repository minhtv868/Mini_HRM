using Microsoft.EntityFrameworkCore;

namespace Web.Application.Interfaces.Repositories.Finances
{
    public interface IEntityTypeConfigurationFinanceDbContext<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
    }
}
