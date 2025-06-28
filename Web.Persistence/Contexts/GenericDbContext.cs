using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Persistence.Contexts
{
    public class GenericDbContext : DbContext
	{
		private string _dbName;
        public readonly ILoggerFactory _loggerFactory;
        IConfiguration _configuration;
        public GenericDbContext(string dbName, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _dbName = dbName;
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) 
            {
                var connectionString = _configuration.GetConnectionString(_dbName);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
