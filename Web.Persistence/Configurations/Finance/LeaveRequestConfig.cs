using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class LeaveRequestConfig : IEntityTypeConfigurationFinanceDbContext<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("LeaveRequests", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.LeaveRequestId);
        }
    }
}
