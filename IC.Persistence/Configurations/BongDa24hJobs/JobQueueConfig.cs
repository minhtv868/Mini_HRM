using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hJobs
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
