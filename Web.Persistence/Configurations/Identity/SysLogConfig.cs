using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
	public class SysLogConfig : IEntityTypeConfiguration<SysLog>
	{
		public void Configure(EntityTypeBuilder<SysLog> builder)
		{
			builder.ToTable("SysLogs");
			builder.HasKey(x => x.Id);
			builder.Property(t => t.SourceContext)
				.HasMaxLength(255).IsUnicode(false);
			builder.Property(t => t.Application)
				.HasMaxLength(50).IsUnicode(false);
		}
	}
}
