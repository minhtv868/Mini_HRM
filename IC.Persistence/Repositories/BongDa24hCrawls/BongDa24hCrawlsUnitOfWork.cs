using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Persistence.Contexts;

namespace IC.Persistence.Repositories.BongDa24hCrawls
{
	public class BongDa24hCrawlsUnitOfWork(BongDa24hCrawlDbContext dbContext) : UnitOfWork(dbContext), IBongDa24HCrawlUnitOfWork
	{
	}
}
