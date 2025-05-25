using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.BongDa24hCrawls
{
	public class TemporaryDataConfig : IEntityTypeConfigurationBongDa24hCrawlDbContext<TemporaryData>
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
