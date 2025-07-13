using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Crawls;

namespace Web.Persistence.Configurations.BongDa24hCrawls
{
    internal class MappingDataConfig : IEntityTypeConfigurationFinanceDbContext<MappingData>
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
