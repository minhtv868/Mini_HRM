using IC.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IC.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;

        Task<int> Save(CancellationToken cancellationToken);
        Task<int> SaveWithoutAudit(CancellationToken cancellationToken);
        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

        Task Rollback();
		Task ChangeTrackerClear();
	}
}
