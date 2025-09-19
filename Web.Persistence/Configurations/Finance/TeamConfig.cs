using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class TeamConfig : IEntityTypeConfigurationFinanceDbContext<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.TeamId);
        }
    }
}
