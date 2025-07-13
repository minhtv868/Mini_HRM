using Web.Application.Interfaces.Repositories;
using Web.Persistence.Contexts;

namespace Web.Persistence.Repositories.Identity
{
    public class IdentityUnitOfWork : UnitOfWork, IIdentityUnitOfWork
    {
        public IdentityUnitOfWork(FinanceDbContext dbContext) : base(dbContext)
        {
        }
    }
}
