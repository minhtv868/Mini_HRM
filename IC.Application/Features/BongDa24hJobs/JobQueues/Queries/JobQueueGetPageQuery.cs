using AutoMapper;
using AutoMapper.QueryableExtensions;
using IC.Application.DTOs.MediatR;
using IC.Application.Extensions;
using IC.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using IC.Shared;
using MediatR;
using System.Linq.Dynamic.Core;

namespace IC.Application.Features.BongDa24hJobs.JobQueues.Queries
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
