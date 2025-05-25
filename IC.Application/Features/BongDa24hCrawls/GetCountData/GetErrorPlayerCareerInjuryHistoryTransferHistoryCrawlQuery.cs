using AutoMapper;
using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using IC.Domain.Enums.Bongda;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC.Application.Features.BongDa24hCrawls.GetCountData
{
    public class GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQuery : IRequest<int>
    {
    }
    internal class GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQueryHandler : IRequestHandler<GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQuery, int>
    {
        private readonly IMapper _mapper;
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        public GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQueryHandler(IMapper mapper, IBongDa24HCrawlUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public Task<int> Handle(GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQuery request, CancellationToken cancellationToken)
        {
            var result1 = _unitOfWork.Repository<PlayerCareerCrawl>().Entities.AsNoTracking().Where(x => x.ProcessResult != "OK").Count();
            var result2 = _unitOfWork.Repository<PlayerInjuryHistoryCrawl>().Entities.AsNoTracking().Where(x => x.ProcessResult != "OK").Count();
            var result3 = _unitOfWork.Repository<PlayerTransferHistoryCrawl>().Entities.AsNoTracking().Where(x => x.ProcessResult != "OK").Count();
            int result = result1 + result2 + result3;
            return Task.FromResult(result);
        }
    }
}
