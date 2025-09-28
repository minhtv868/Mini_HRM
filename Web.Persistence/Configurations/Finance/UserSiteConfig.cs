using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class UserSiteConfig : IEntityTypeConfigurationFinanceDbContext<UserSite>
    {
        public void Configure(EntityTypeBuilder<UserSite> builder)
        {
            builder.ToTable("UserSites", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.UserSiteId);
        }
    }
}
