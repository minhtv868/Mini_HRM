using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hJobs
{
    internal class JobConfig : IEntityTypeConfigurationBongDa24hJobDbContext<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
