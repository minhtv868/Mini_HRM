using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Jobs;

namespace Web.Persistence.Configurations.Finances
{
    internal class JobConfig : IEntityTypeConfigurationFinanceDbContext<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs", t =>
            {
                // t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
