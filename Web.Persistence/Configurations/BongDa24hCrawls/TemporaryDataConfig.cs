using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.BongDa24hCrawls
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
