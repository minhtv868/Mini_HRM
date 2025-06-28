using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            //builder.ToTable("Roles");
            builder.Property(t => t.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.NormalizedName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.ConcurrencyStamp)
                .HasMaxLength(200);
            builder.Property(t => t.Description)
                .HasMaxLength(200);
        }
    }
}
