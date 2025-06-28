using Web.Application.Common.Jobs;
using Web.Application.Interfaces.Repositories;
using Web.Domain.Entities.Identity;
using MediatR;

namespace Web.Application.Jobs.ProcessLog
{
	[PreventConcurrentExecutionJobFilter]
	public record SysLogResetJob : IRequest
	{
		
	}
	internal class SysLogResetJobHandler : IRequestHandler<SysLogResetJob>
	{
		private readonly IIdentityUnitOfWork _uow;

		public SysLogResetJobHandler(IIdentityUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task Handle(SysLogResetJob command, CancellationToken cancellationToken)
		{
			var sqlDel = $"DELETE FROM SysLogs WHERE TimeStamp < '{DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss")}'";
			await _uow.Repository<SysLog>().ExecNoneQuerySql(sqlDel);
		}
	}
}
