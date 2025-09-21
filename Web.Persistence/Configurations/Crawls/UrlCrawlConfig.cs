using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    public class UrlCrawlConfig : IEntityTypeConfigurationFinanceDbContext<UrlCrawl>
    {
        public void Configure(EntityTypeBuilder<UrlCrawl> builder)
        {
            builder.ToTable("UrlCrawls", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Url).HasMaxLength(1024).IsRequired();
            //builder.Property(x => x.UrlDesc).HasMaxLength(250);
            //builder.Property(x => x.UrlType).HasMaxLength(50);
            //builder.Property(x => x.UrlGroup).HasMaxLength(50);
            //builder.Property(x => x.BatchCode).HasMaxLength(50);
            //builder.Property(x => x.CrawlResult).HasMaxLength(250);
        }
    }
}
