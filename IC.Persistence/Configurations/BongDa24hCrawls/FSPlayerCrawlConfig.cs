
using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hCrawls
{
    internal class FSPlayerCrawlConfig : IEntityTypeConfigurationBongDa24hCrawlDbContext<FSPlayerCrawl>
    {
        public void Configure(EntityTypeBuilder<FSPlayerCrawl> builder)
        {
            builder.ToTable("FSPlayerCrawls", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.PlayerName).HasMaxLength(100);
            builder.Property(x => x.FSPlayerId).HasMaxLength(50);
            builder.Property(x => x.FSPlayerUrl).HasMaxLength(100);
            builder.Property(x => x.UrlCrawl).HasMaxLength(1024);
            builder.Property(x => x.BatchCode).HasMaxLength(50);
            builder.Property(x => x.ProcessResult).HasMaxLength(250);
        }
    }
}
