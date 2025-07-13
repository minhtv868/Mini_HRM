using Web.Application.Interfaces.Repositories.Finances;
using Web.Persistence.Contexts;

namespace Web.Persistence.Repositories.Finances
{
	public class FinanceUnitOfWork(FinanceDbContext dbContext) : UnitOfWork(dbContext), IFinanceUnitOfWork
	{
	}
}
