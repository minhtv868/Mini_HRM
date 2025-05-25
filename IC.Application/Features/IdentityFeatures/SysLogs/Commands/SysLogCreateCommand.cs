using AutoMapper;
using IC.Application.Common.Constants;
using IC.Application.Common.Mappings;
using IC.Application.Interfaces.Repositories;
using IC.Domain.Entities.Identity;
using IC.Shared;
using MediatR;

namespace IC.Application.Features.IdentityFeatures.SysLogs.Commands
{
    public record SysLogCreateCommand : IRequest<Result<int>>
	{
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string SourceContext { get; set; }
        public string Application { get; set; }
		public SysLogCreateCommand(string sourceContext, string errorLevel, string message, string exception = "")
		{
            Message = message;
            Level = errorLevel;
            Exception = exception;
			SourceContext = sourceContext;
			Application = AppConstant.Application;
        }
    }

	internal class SysLogCreateCommandHandler : IRequestHandler<SysLogCreateCommand, Result<int>>
	{
		private readonly IIdentityUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public SysLogCreateCommandHandler(IIdentityUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(SysLogCreateCommand command, CancellationToken cancellationToken)
		{
			var sysLog = new SysLog()
			{
                Message = command.Message,
				Level = command.Level,
				Exception = command.Exception,
				SourceContext = command.SourceContext,
				Application = command.Application,
                TimeStamp = DateTime.Now
            };

			await _unitOfWork.Repository<SysLog>().AddAsync(sysLog);
			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(sysLog.Id, "Created.");
		}
	}
}
