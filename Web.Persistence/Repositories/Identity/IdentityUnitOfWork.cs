using Web.Application.Interfaces.Repositories;
using Web.Domain.Common;
using Web.Persistence.Contexts;
using System.Collections;

namespace Web.Persistence.Repositories.Identity
{
    public class IdentityUnitOfWork : UnitOfWork, IIdentityUnitOfWork
    {
        public IdentityUnitOfWork(WebJobDbContext dbContext) : base(dbContext)
        {
        }
    }
}
