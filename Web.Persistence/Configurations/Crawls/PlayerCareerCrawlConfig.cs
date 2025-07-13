using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Crawls;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    internal class PlayerCareerCrawlConfig : IEntityTypeConfigurationFinanceDbContext<PlayerCareerCrawl>
    {
        public void Configure(EntityTypeBuilder<PlayerCareerCrawl> builder)
        {
            builder.ToTable("PlayerCareerCrawls", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UrlCrawl).HasMaxLength(1024).IsRequired();
            builder.Property(x => x.FootballSeasonName).HasMaxLength(100);
            builder.Property(x => x.TeamName).HasMaxLength(100);
            builder.Property(x => x.TeamLogo).HasMaxLength(255);
            builder.Property(x => x.TeamUrl).HasMaxLength(255);
            builder.Property(x => x.LeagueName).HasMaxLength(100);
            builder.Property(x => x.LeagueLogo).HasMaxLength(255);
            builder.Property(x => x.LeagueUrl).HasMaxLength(255);
            builder.Property(x => x.BatchCode).HasMaxLength(50);
            builder.Property(x => x.ProcessResult).HasMaxLength(250);
        }
    }
}
