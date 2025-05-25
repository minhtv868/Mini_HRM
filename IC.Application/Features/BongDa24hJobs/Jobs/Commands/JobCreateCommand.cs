using AutoMapper;
using IC.Application.Common.Mappings;
using IC.Application.DTOs.MediatR;
using IC.Application.Interfaces;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using IC.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IC.Application.Features.BongDa24hJobs.Jobs.Commands
{
	public record JobCreateCommand : BaseCreateCommand, IRequest<Result<int>>, IMapFrom<Job>
	{ 
		public string JobName { get; set; }
		public string JobClassName { get; set; }
		public string JobClassType { get; set; } 
	}
	internal class JobCreateCommandHandler : IRequestHandler<JobCreateCommand, Result<int>>
	{
		private readonly IBongDa24hJobUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public JobCreateCommandHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}
		public async Task<Result<int>> Handle(JobCreateCommand command, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(command.JobClassType))
			{
				return await Result<int>.FailureAsync("Hãy nhập Class Type.");
			}
			var isExists = await _unitOfWork.Repository<Job>().Entities.AnyAsync(x => x.JobClassType == command.JobClassType);

			if (isExists)
			{
				return await Result<int>.FailureAsync("Class Type đã tồn tại.");
			}

			var entity = _mapper.Map<Job>(command);

			entity.CrUserId = _currentUserService.UserId;

			entity.CrDateTime = DateTime.Now;

			await _unitOfWork.Repository<Job>().AddAsync(entity);
			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(entity.Id, "Tạo Job thành công."); 
		}
	}
}
