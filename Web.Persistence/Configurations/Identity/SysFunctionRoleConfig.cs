using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
    public class SysFunctionRoleConfig : IEntityTypeConfiguration<SysFunctionRole>
    {
        public void Configure(EntityTypeBuilder<SysFunctionRole> builder)
        {
            builder.ToTable("SysFunctionRoles");
            builder.HasKey(x => new { x.SysFunctionId, x.RoleId });
        }
    }
}
