using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class DepartmentConfig : IEntityTypeConfigurationFinanceDbContext<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.DepartmentId);
        }
    }
}
