using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class AttendanceConfig : IEntityTypeConfigurationFinanceDbContext<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.AttendanceId);
        }
    }
}
