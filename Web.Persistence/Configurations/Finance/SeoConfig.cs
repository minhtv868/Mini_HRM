using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class SeoConfig : IEntityTypeConfigurationFinanceDbContext<Seo>
    {
        public void Configure(EntityTypeBuilder<Seo> builder)
        {
            builder.ToTable("Seos", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.SeoId);
        }
    }
}
