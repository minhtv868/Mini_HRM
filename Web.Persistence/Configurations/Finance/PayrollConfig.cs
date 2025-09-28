using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class PayrollConfig : IEntityTypeConfigurationFinanceDbContext<Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll> builder)
        {
            builder.ToTable("Payrolls", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.PayrollId);
        }
    }
}
