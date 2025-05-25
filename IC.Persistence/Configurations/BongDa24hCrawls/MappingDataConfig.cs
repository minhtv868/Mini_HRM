using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hCrawls
{
    internal class MappingDataConfig : IEntityTypeConfigurationBongDa24hCrawlDbContext<MappingData>
    {
        public void Configure(EntityTypeBuilder<MappingData> builder)
        {
            builder.ToTable("MappingDatas", t =>
            {
                t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.DataSouceName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.DataId).IsRequired();
        }
    }
}
