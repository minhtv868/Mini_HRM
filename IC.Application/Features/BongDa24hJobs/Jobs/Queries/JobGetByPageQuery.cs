using AutoMapper;
using IC.Application.DTOs.MediatR;
using IC.Application.Interfaces.Repositories.Identity;
using IC.Shared;
using MediatR;
using IC.Application.Features.BongDa24hJobs.Jobs.DTOs;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using IC.Application.Extensions;

namespace IC.Application.Features.BongDa24hJobs.Jobs.Queries
{
	public record JobGetByPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<JobGetByPageDto>>
	{
	}

	public class JobGetByPageQueryHandler : IRequestHandler<JobGetByPageQuery, PaginatedResult<JobGetByPageDto>>
	{
		private readonly IBongDa24hJobUnitOfWork _unitOfWork;
		private readonly IUserRepo _userRepo;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;
		public JobGetByPageQueryHandler(IBongDa24hJobUnitOfWork unitOfWork,
			IUserRepo userRepo,
			IMapper mapper,
			IMediator mediator)
		{
			_unitOfWork = unitOfWork;
			_userRepo = userRepo;
			_mapper = mapper;
			_mediator = mediator;
		}

		public async Task<PaginatedResult<JobGetByPageDto>> Handle(JobGetByPageQuery request, CancellationToken cancellationToken)
		{
			var query = _unitOfWork.Repository<Job>().Entities.AsNoTracking();
			 

			if (!string.IsNullOrWhiteSpace(request.Keywords))
			{
				query = query.Where(x => x.JobClassType.Contains(request.Keywords) || x.JobName.Contains(request.Keywords));
			} 
			query.OrderByDescending(x => x.CrDateTime); 

			var result = await query.ProjectTo<JobGetByPageDto>(_mapper.ConfigurationProvider)
				.ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

			if (result != null && result.Data.Count > 0)
			{
				List<int> userIds = result.Data.Where(x=>x.CrUserId.HasValue).Select(x => x.CrUserId.Value).ToList();

				var usersList = await _userRepo.GetListUser(userIds);

				Domain.Entities.Identity.User actionUser = null;

				foreach (var item in result.Data)
				{
					if (usersList != null && usersList.Any())
					{
						actionUser = null;

						actionUser ??= usersList.FirstOrDefault(x => x.Id == item.CrUserId);
						if (actionUser != null)
						{
							item.CrUserName = actionUser.FullName ?? actionUser.UserName;
						} 
					}
				}

			}
			return result;
		}
	}
}