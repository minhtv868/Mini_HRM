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
    public class GetErrorFSPlayerCrawlQuery : IRequest<int>
    {
    }
    internal class GetErrorFSPlayerCrawlQueryHandler : IRequestHandler<GetErrorFSPlayerCrawlQuery, int>
    {
        private readonly IMapper _mapper;
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        public GetErrorFSPlayerCrawlQueryHandler(IMapper mapper, IBongDa24HCrawlUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public Task<int> Handle(GetErrorFSPlayerCrawlQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Repository<FSPlayerCrawl>().Entities.AsNoTracking().Where(x => x.ProcessStatusId == 0 || x.ProcessStatusId == (byte)ProcessStatusEnum.CrawlError || x.ProcessStatusId == (byte)ProcessStatusEnum.ParseError).Count();
            return Task.FromResult(result);
        }
    }
}
