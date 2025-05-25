using IC.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IC.Persistence.Configurations.Identity
{
    public class UserLogConfig : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.ToTable("UserLogs");
            builder.Property(t => t.UserName)
                .HasMaxLength(100);
            builder.Property(t => t.FromIP)
                .HasMaxLength(100);
            builder.Property(t => t.UserAction)
                .HasMaxLength(50);
            builder.Property(t => t.ActionStatus)
                .HasMaxLength(50);
            builder.Property(t => t.CrDateTime)
                .HasDefaultValueSql("getdate()");
        }
    }
}
