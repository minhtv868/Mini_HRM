using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Persistence.Configurations.Finances
{
    internal class ArticleConfig : IEntityTypeConfigurationFinanceDbContext<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Articles", t =>
            {
                //  t.ExcludeFromMigrations();
            });

            builder.HasKey(x => x.Id);
        }
    }
}
