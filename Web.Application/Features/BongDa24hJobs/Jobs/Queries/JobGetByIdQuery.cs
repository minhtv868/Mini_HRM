using AutoMapper;
using AutoMapper.QueryableExtensions;
using Web.Application.Features.BongDa24hJobs.Jobs.DTOs;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Entities.Jobs; 
using Web.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore; 
namespace Web.Application.Features.BongDa24hJobs.Jobs.Queries
{ 
	public record JobGetByIdQuery : IRequest<Result<JobGetByIdDto>>
	{
		public int Id { get; set; } 
	}

	internal class JobGetByIdQueryHandler : IRequestHandler<JobGetByIdQuery, Result<JobGetByIdDto>>
	{
		private readonly IBongDa24hJobUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		public JobGetByIdQueryHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_mediator = mediator;
		}

		public async Task<Result<JobGetByIdDto>> Handle(JobGetByIdQuery request, CancellationToken cancellationToken)
		{
			var dataGetByIdDto = await _unitOfWork.Repository<Job>().Entities.AsNoTracking()
				.Where(x => x.Id == request.Id)
				.ProjectTo<JobGetByIdDto>(_mapper.ConfigurationProvider)
				.FirstOrDefaultAsync(cancellationToken);

			if (dataGetByIdDto == null)
			{
				return await Result<JobGetByIdDto>.FailureAsync($"Dữ liệu Id <b>{request.Id}</b> không tồn tại.");
			}
			 

			return await Result<JobGetByIdDto>.SuccessAsync(dataGetByIdDto);
		}
	}
}
