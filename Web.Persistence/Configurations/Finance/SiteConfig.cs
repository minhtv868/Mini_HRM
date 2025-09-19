using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class SiteConfig : IEntityTypeConfigurationFinanceDbContext<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.ToTable("Sites", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.SiteId);
        }
    }
}
