using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class LeagueConfig : IEntityTypeConfigurationFinanceDbContext<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.ToTable("Leagues", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.LeagueId);
        }
    }
}
