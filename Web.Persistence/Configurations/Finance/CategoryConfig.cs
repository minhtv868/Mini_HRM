using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class CategoryConfig : IEntityTypeConfigurationFinanceDbContext<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.CategoryId);
        }
    }
}
