using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Crawls;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    public class TemporaryDataConfig : IEntityTypeConfigurationFinanceDbContext<TemporaryData>
    {
        public void Configure(EntityTypeBuilder<TemporaryData> builder)
        {
            builder.ToTable("TemporaryDatas", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
