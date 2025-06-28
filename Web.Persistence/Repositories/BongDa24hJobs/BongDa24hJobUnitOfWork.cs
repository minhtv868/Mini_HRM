using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Persistence.Contexts;

namespace Web.Persistence.Repositories.BongDa24hJobs
{
    public class BongDa24hJobUnitOfWork(BongDa24hJobDbContext dbContext) : UnitOfWork(dbContext), IBongDa24hJobUnitOfWork
    {
    }
}
