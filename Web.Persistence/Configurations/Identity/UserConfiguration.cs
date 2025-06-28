using Web.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web.Persistence.Configurations.Identity
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(t => t.UserName)
               .HasMaxLength(200)
               .IsRequired();
            builder.Property(t => t.NormalizedUserName)
                .HasMaxLength(200);
            builder.Property(t => t.PasswordHash)
                .HasMaxLength(200);
            builder.Property(t => t.FullName)
                .HasMaxLength(200);
            builder.Property(t => t.Email)
                .HasMaxLength(200);
            builder.Property(t => t.NormalizedEmail)
                .HasMaxLength(200);
            builder.Property(t => t.PhoneNumber)
                .HasMaxLength(50);
            builder.Property(t => t.Address)
                .HasMaxLength(200);
            builder.Property(t => t.Notes)
                .HasMaxLength(200);
            builder.Property(t => t.AvatarPath)
                .HasMaxLength(200);
            builder.Property(t => t.ConcurrencyStamp)
                .HasMaxLength(200);
            builder.Property(t => t.SecurityStamp)
                .HasMaxLength(200);
            builder.Property(t => t.PhoneNumberConfirmed)
                .HasDefaultValue(false);
            builder.Property(t => t.EmailConfirmed)
                .HasDefaultValue(false);
            builder.Property(t => t.DefaultActionId)
                .HasDefaultValue(0);
            builder.Property(t => t.TwoFactorEnabled)
                .HasDefaultValue(false);
            builder.Property(t => t.LockoutEnabled)
                .HasDefaultValue(false);
            builder.Property(t => t.IsEnabled)
                .HasDefaultValue(true);
            builder.Property(t => t.AccessFailedCount)
               .HasDefaultValue(0);
            builder.Property(t => t.CrDateTime)
               .HasDefaultValueSql("getdate()");
        }
    }
}
