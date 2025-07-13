using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Jobs;

namespace Web.Persistence.Configurations.Finances
{
    internal class JobQueueConfig : IEntityTypeConfigurationFinanceDbContext<JobQueue>
    {
        public void Configure(EntityTypeBuilder<JobQueue> builder)
        {
            builder.ToTable("JobQueues", t =>
            {
                //   t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
