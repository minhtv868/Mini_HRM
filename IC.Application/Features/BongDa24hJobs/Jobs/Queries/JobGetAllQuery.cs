using AutoMapper;
using AutoMapper.QueryableExtensions;
using IC.Application.Features.BongDa24hJobs.Jobs.DTOs;
using IC.Application.Interfaces.Repositories.BongDa24hJobs;
using IC.Domain.Entities.BongDa24hJobs;
using MediatR;
using Microsoft.EntityFrameworkCore; 

namespace IC.Application.Features.BongDa24hJobs.Jobs.Queries
{ 
    public record JobGetAllQuery : IRequest<List<JobGetAllDto>>
    {
    }

    internal class JobGetAllQueryHandler : IRequestHandler<JobGetAllQuery, List<JobGetAllDto>>
    {
        private readonly IBongDa24hJobUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobGetAllQueryHandler(IBongDa24hJobUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<JobGetAllDto>> Handle(JobGetAllQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Job>().Entities.AsNoTracking();

            var result = await query
                   .ProjectTo<JobGetAllDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return result;
        }
    }
}
