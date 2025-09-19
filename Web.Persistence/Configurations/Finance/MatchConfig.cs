using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class MatchConfig : IEntityTypeConfigurationFinanceDbContext<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matchs", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.MatchId);
        }
    }
}
