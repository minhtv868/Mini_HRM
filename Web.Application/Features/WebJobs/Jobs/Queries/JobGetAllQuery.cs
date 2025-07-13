using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finances.Jobs.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Jobs;

namespace Web.Application.Features.Finances.Jobs.Queries
{
    public record JobGetAllQuery : IRequest<List<JobGetAllDto>>
    {
    }

    internal class JobGetAllQueryHandler : IRequestHandler<JobGetAllQuery, List<JobGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper)
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
