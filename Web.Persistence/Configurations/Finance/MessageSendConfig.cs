using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class MessageSendConfig : IEntityTypeConfigurationFinanceDbContext<MessageSend>
    {
        public void Configure(EntityTypeBuilder<MessageSend> builder)
        {
            builder.ToTable("MessageSends", t =>
            {
                //t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.MessageSendId);
        }
    }
}
