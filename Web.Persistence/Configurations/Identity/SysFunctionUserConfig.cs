using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
	public class SysFunctionUserConfig : IEntityTypeConfiguration<SysFunctionUser>
	{
		public void Configure(EntityTypeBuilder<SysFunctionUser> builder)
		{
			builder.ToTable("SysFunctionUsers");
			builder.HasKey(x => new { x.SysFunctionId, x.UserId });
		}
	}
}
