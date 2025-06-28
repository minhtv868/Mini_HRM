using AutoMapper;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Application.Interfaces;
using Web.Shared;
using MediatR;
using Web.Domain.Entities.Jobs;
using Web.Application.Features.BongDa24hJobs.Jobs.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hJobs.Jobs.Commands
{
	public record JobEditCommand : IRequest<Result<int>>, IMapFrom<Job>, IMapFrom<JobGetByIdDto>
	{
		public int Id { get; set; }
		public string JobName { get; set; }
		public string JobClassName { get; set; }
		public string JobClassType { get; set; }
	}
	internal class JobEditCommandHandler : IRequestHandler<JobEditCommand, Result<int>>
	{
		private readonly IBongDa24hJobUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public JobEditCommandHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}
		public async Task<Result<int>> Handle(JobEditCommand command, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(command.JobClassType))
			{
				return await Result<int>.FailureAsync("Hãy nhập Class Type.");
			}
			var isExists = await _unitOfWork.Repository<Job>().Entities.AnyAsync(x => x.JobClassType == command.JobClassType && x.Id != command.Id);

			if (isExists)
			{
				return await Result<int>.FailureAsync("Class Type đã tồn tại.");
			}

			var entity = _mapper.Map<Job>(command);
			 

			await _unitOfWork.Repository<Job>().UpdateFieldsAsync(entity, x=>x.JobName, x=>x.JobClassType, x=>x.JobClassName);

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(entity.Id, "Cập nhật thành công.");
		}
	}
}