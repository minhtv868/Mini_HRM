//using Web.Application.Interfaces.Repositories;
//using Web.Persistence.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Web.Persistence.Repositories
//{
//    public class GenericDbService : IGenericDbService
//    {
//        private IConfiguration _configuration;
//        public readonly ILoggerFactory _loggerFactory;
//        public GenericDbService(IConfiguration configuration, ILoggerFactory loggerFactory)
//        {
//            _configuration = configuration;
//            _loggerFactory = loggerFactory;
//        }

//        public async Task<string> ExecQuerySqlReturnScalar(string connectionString, string sql, params object[] parameters)
//        {
//            var dbContext = new GenericDbContext(connectionString, _loggerFactory, _configuration);

//            var result = await dbContext.Database.SqlQueryRaw<string>(sql, parameters).ToListAsync();
//            return result?.FirstOrDefault();
//        }
//    }
//}
