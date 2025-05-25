using IC.Application.Common.Jobs;
using IC.Application.Interfaces.Repositories;
using IC.Domain.Entities.Identity;
using MediatR;

namespace IC.Application.Jobs.ProcessLog
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
