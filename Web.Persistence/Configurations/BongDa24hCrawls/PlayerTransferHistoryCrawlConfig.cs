using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    internal class PlayerTransferHistoryCrawlConfig : IEntityTypeConfigurationBongDa24hCrawlDbContext<PlayerTransferHistoryCrawl>
    {
        public void Configure(EntityTypeBuilder<PlayerTransferHistoryCrawl> builder)
        {
            builder.ToTable("PlayerTransferHistoryCrawls", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UrlCrawl).HasMaxLength(1024).IsRequired();
            builder.Property(x => x.FromTeamName).HasMaxLength(100);
            builder.Property(x => x.FromTeamLogo).HasMaxLength(255);
            builder.Property(x => x.FromTeamUrl).HasMaxLength(255);
            builder.Property(x => x.ToTeamName).HasMaxLength(100);
            builder.Property(x => x.ToTeamLogo).HasMaxLength(255);
            builder.Property(x => x.ToTeamUrl).HasMaxLength(255);
            builder.Property(x => x.Type).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Fee).HasMaxLength(100);
            builder.Property(x => x.BatchCode).HasMaxLength(50);
            builder.Property(x => x.ProcessResult).HasMaxLength(250);
        }
    }
}
