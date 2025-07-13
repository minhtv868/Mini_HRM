using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class MessageTemplateConfig : IEntityTypeConfigurationFinanceDbContext<MessageTemplate>
    {
        public void Configure(EntityTypeBuilder<MessageTemplate> builder)
        {
            builder.ToTable("MessageTemplates", t =>
            {
                //  t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.MessageTemplateId);
        }
    }
}
