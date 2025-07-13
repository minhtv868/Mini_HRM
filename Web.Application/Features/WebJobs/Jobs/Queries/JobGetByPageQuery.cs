using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finances.Jobs.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Domain.Entities.Jobs;
using Web.Shared;

namespace Web.Application.Features.Finances.Jobs.Queries
{
    public record JobGetByPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<JobGetByPageDto>>
    {
    }

    public class JobGetByPageQueryHandler : IRequestHandler<JobGetByPageQuery, PaginatedResult<JobGetByPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public JobGetByPageQueryHandler(IFinanceUnitOfWork unitOfWork,
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
                List<int> userIds = result.Data.Where(x => x.CrUserId.HasValue).Select(x => x.CrUserId.Value).ToList();

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