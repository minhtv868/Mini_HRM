using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
    public class SysFunctionConfig : IEntityTypeConfiguration<SysFunction>
    {
        public void Configure(EntityTypeBuilder<SysFunction> builder)
        {
            builder.ToTable("SysFunctions");
            builder.Property(t => t.FunctionName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(t => t.FunctionDesc)
                .HasMaxLength(200);
            builder.Property(t => t.Url)
                .HasMaxLength(200);
            builder.Property(t => t.IconPath)
                .HasMaxLength(200);
            builder.Property(t => t.CssMenuActive)
                .HasMaxLength(200);
            builder.Property(t => t.CssMenuOpen)
                .HasMaxLength(200);
            builder.Property(t => t.ParentItemId)
                .HasDefaultValue(0);
            builder.Property(t => t.DisplayOrder)
                .HasDefaultValue(1);
            builder.Property(t => t.TreeOrder)
                .HasDefaultValue(0);
            builder.Property(t => t.TreeLevel)
                .HasDefaultValue(0);
            builder.Property(t => t.IsEnable)
                .HasDefaultValue(true);
            builder.Property(t => t.IsShow)
                .HasDefaultValue(true);
            builder.Property(t => t.HasChild)
                .HasDefaultValue(false);
        }
    }
}
