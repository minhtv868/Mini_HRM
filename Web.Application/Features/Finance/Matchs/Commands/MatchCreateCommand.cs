using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;
using Web.Shared.Helpers;

namespace Web.Application.Features.Finance.Matchs.Commands
{
    public class MatchCreateCommand : IRequest<Result<int>>, IMapFrom<Match>
    {

        [DisplayName("Thời gian")]
        public DateTime EstimateStartTime { get; set; }
        [DisplayName("Thời gian")]
        public string EstimateStartTimeText { get; set; }

        [DisplayName("Đội chủ nhà")]
        public short? HomeId { get; set; }

        [DisplayName("Đội khách")]
        public short? AwayId { get; set; }

        [DisplayName("Giải đấu")]
        public short? LeagueId { get; set; }

        [DisplayName("Tên đội chủ nhà")]
        public string HomeName { get; set; }
        [DisplayName("Tên đội khách")]
        public string AwayName { get; set; }
        [DisplayName("Logo đội chủ nhà")]
        public string HomeLogoPath { get; set; }
        [DisplayName("Logo đội khách")]
        public string AwayLogoPath { get; set; }
        [DisplayName("Bàn thắng đội chủ nhà")]
        public byte? HomeGoals { get; set; }
        [DisplayName("Bàn thắng đội khách")]
        public byte? AwayGoals { get; set; }

        [DisplayName("Sân vận động")]
        public string StadiumName { get; set; }
        [DisplayName("Tên giải đấu")]
        public string LeagueName { get; set; }
        [DisplayName("Logo giải đấu")]
        public string LeagueImage { get; set; }
        [DisplayName("Trực tiếp")]
        public bool IsLive { get; set; }
        [DisplayName("Nổi bật")]
        public bool IsHot { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class MatchCreateCommandHandler : IRequestHandler<MatchCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MatchCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MatchCreateCommand command, CancellationToken cancellationToken)
        {
            //var dataAny = _unitOfWork.Repository<Match>().Entities.FirstOrDefault(x => x.H.Trim().ToLower().Equals(command.MessageName.Trim().ToLower()));
            //if (Match != null)
            //{
            //    return await Result<int>.FailureAsync($"Match đã tồn tại");
            //}

            var entity = _mapper.Map<Match>(command);
            if (!string.IsNullOrEmpty(command.EstimateStartTimeText))
            {
                entity.EstimateStartTime = command.EstimateStartTimeText.StrToDateTime("dd-MM-yyyy HH:mm:ss");
            }
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Match>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
