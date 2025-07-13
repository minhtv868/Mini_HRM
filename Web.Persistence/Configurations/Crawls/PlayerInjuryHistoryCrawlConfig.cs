using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Crawls;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    internal class PlayerInjuryHistoryCrawlConfig : IEntityTypeConfigurationFinanceDbContext<PlayerInjuryHistoryCrawl>
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
