using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.BongDa24hJobs
{
    internal class JobQueueConfig : IEntityTypeConfigurationBongDa24hJobDbContext<JobQueue>
    {
        public void Configure(EntityTypeBuilder<JobQueue> builder)
        {
            builder.ToTable("JobQueues", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
