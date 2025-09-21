using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.UrlCrawls.DTOs;
using Web.Application.Interfaces.Repositories.Finances;

namespace Web.Application.Features.Finance.UrlCrawls.Queries
{
    public class UrlCrawlGetAllQuery : IRequest<List<UrlCrawlGetAllDto>>
    {
        public byte? DataType { get; set; }
    }
    internal class UrlCrawlGetAllQueryHandler : IRequestHandler<UrlCrawlGetAllQuery, List<UrlCrawlGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public UrlCrawlGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<UrlCrawlGetAllDto>> Handle(UrlCrawlGetAllQuery request, CancellationToken cancellationToken)
        {
            //var query = _unitOfWork.Repository<UrlCrawl>().Entities.AsNoTracking();
            //var result = await query
            //     .ProjectTo<UrlCrawlGetAllDto>(_mapper.ConfigurationProvider)
            //     .ToListAsync(cancellationToken);
            //return result;
            var list = new List<UrlCrawlGetAllDto>
    {
        new UrlCrawlGetAllDto { Id = 1, Name = "Premier League", Url = "https://prod-public-api.livescore.com/v1/api/app/stage/soccer/england/premier-league/7.00", DataType = 1, DataId = 1001, IsActive = true, CrDateTime = DateTime.Now },
        //new UrlCrawlGetAllDto { Id = 2, Name = "La Liga", Url = "https://api.livescore.com/laliga", DataType = 1, DataId = 1002, IsActive = true, CrDateTime = DateTime.Now },
        //new UrlCrawlGetAllDto { Id = 3, Name = "Serie A", Url = "https://api.livescore.com/seriea", DataType = 1, DataId = 1003, IsActive = true, CrDateTime = DateTime.Now },
        //new UrlCrawlGetAllDto { Id = 4, Name = "Bundesliga", Url = "https://api.livescore.com/bundesliga", DataType = 1, DataId = 1004, IsActive = true, CrDateTime = DateTime.Now },
        //new UrlCrawlGetAllDto { Id = 5, Name = "Ligue 1", Url = "https://api.livescore.com/ligue1", DataType = 1, DataId = 1005, IsActive = true, CrDateTime = DateTime.Now }
    };

            return list;
        }
    }
}
