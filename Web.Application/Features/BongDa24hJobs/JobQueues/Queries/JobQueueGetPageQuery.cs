using AutoMapper;
using AutoMapper.QueryableExtensions;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Entities.Jobs;
using Web.Shared;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Web.Application.Features.BongDa24hJobs.JobQueues.Queries
{
    public record JobQueueGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<JobQueueGetPageDto>>
    {
    }
    internal class JobQueueGetPageQueryHandler : IRequestHandler<JobQueueGetPageQuery, PaginatedResult<JobQueueGetPageDto>>
    {
        private readonly IBongDa24hJobUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobQueueGetPageQueryHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<JobQueueGetPageDto>> Handle(JobQueueGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<JobQueue>().Entities;

            if (!string.IsNullOrEmpty(queryInput.Keywords))
            {
                query = query.Where(x => x.DataSouceName == queryInput.Keywords || x.JobName == queryInput.Keywords);
            }

            var result = await query.OrderByDescending(x => x.Id)
                .ProjectTo<JobQueueGetPageDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);

            return result;
        }
    }
}
