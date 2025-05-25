using IC.Application.Interfaces.Repositories;
using IC.Domain.Common;
using IC.Persistence.Contexts;
using System.Collections;

namespace IC.Persistence.Repositories.Identity
{
    public class IdentityUnitOfWork : UnitOfWork, IIdentityUnitOfWork
    {
        public IdentityUnitOfWork(WebJobDbContext dbContext) : base(dbContext)
        {
        }
    }
}
