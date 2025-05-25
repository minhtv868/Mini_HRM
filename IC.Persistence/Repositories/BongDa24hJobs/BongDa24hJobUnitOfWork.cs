using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Persistence.Contexts;

namespace IC.Persistence.Repositories.BongDa24hJobs
{
    public class BongDa24hJobUnitOfWork(BongDa24hJobDbContext dbContext) : UnitOfWork(dbContext), IBongDa24hJobUnitOfWork
    {
    }
}
