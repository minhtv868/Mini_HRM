using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.BongDa24hJobs
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
