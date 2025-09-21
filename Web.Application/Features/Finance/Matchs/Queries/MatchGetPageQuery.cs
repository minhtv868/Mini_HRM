using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Matchs.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Domain.Enums;
using Web.Shared;
using Web.Shared.Helpers;

namespace Web.Application.Features.Finance.Matchs.Queries
{
    public record MatchGetPageQuery : BaseGetPageQuery, MediatR.IRequest<PaginatedResult<MatchGetPageDto>>
    {
        [DisplayName("Giải đấu")]
        public short? LeagueId { get; set; }
        [DisplayName("Đội bóng")]
        public short? TeamId { get; set; }
        [DisplayName("Trạng thái")]
        public byte? Status { get; set; }
        [DisplayName("Trận đấu")]
        public byte? TimePlaying { get; set; }
        [DisplayName("Thời gian")]
        public string EstimateStartTimeText { get; set; }
    }
    internal class MatchGetPageQueryHandler : IRequestHandler<MatchGetPageQuery, PaginatedResult<MatchGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public MatchGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<MatchGetPageDto>> Handle(MatchGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Match>().Entities;
            // var listSendMethod = _unitOfWork.Repository<SendMethod>().Entities;
            if (queryInput.LeagueId > 0)
            {
                query = query.Where(x => x.LeagueId == queryInput.LeagueId);
            }
            if (queryInput.TeamId > 0)
            {
                query = query.Where(x => x.HomeId == queryInput.TeamId || x.AwayId == queryInput.TeamId);
            }
            if (queryInput.Status > 0)
            {
                query = query.Where(x => x.Status == queryInput.Status);
            }
            if (queryInput.TimePlaying > 0)
            {
                if (queryInput.TimePlaying == (byte)TimePlayingEnum.FT) //Đang diễn ra
                {
                    query = query.Where(x => x.TimePlaying == "FT");
                }
                if (queryInput.TimePlaying == (byte)TimePlayingEnum.NS) //Đang diễn ra
                {
                    query = query.Where(x => x.TimePlaying == "NS");
                }
            }
            if (!string.IsNullOrEmpty(queryInput.Keywords))
            {
                var kw = queryInput.Keywords.Trim();
                query = query.Where(x =>
                    x.HomeName.Contains(kw) ||
                    x.AwayName.Contains(kw) ||
                    x.LeagueName.Contains(kw) ||
                    x.MatchId.ToString().Contains(kw));
            }
            if (!string.IsNullOrEmpty(queryInput.EstimateStartTimeText))
            {
                var date = queryInput.EstimateStartTimeText.ToDateTime().Date;
                query = query.Where(x => x.EstimateStartTime.HasValue &&
                                         x.EstimateStartTime.Value.Date == date);
            }

            var result = await query.OrderByDescending(x => x.EstimateStartTime).ProjectTo<MatchGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);

            await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}
