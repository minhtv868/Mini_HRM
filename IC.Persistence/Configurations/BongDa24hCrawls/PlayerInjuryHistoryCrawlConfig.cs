using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hCrawls
{
    internal class PlayerInjuryHistoryCrawlConfig : IEntityTypeConfigurationBongDa24hCrawlDbContext<PlayerInjuryHistoryCrawl>
    {
        public void Configure(EntityTypeBuilder<PlayerInjuryHistoryCrawl> builder)
        {
            builder.ToTable("PlayerInjuryHistoryCrawls", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UrlCrawl).HasMaxLength(1024).IsRequired();
            builder.Property(x => x.InjuryType).HasMaxLength(50).IsRequired();
            builder.Property(x => x.BatchCode).HasMaxLength(50);
            builder.Property(x => x.ProcessResult).HasMaxLength(250);
        }
    }
}
