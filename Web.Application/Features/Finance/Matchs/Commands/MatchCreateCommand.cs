using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Matchs.Commands
{
    public class MatchCreateCommand : IRequest<Result<int>>, IMapFrom<Match>
    {
        [DisplayName("Tên trận đấu")]
        public DateTime? EstimateStartTime { get; set; }

        public short? HomeId { get; set; }
        public short? AwayId { get; set; }
        public short? LeagueId { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string HomeLogoPath { get; set; }
        public string AwayLogoPath { get; set; }
        public byte? HomeGoals { get; set; }
        public byte? AwayGoals { get; set; }
        public string StadiumName { get; set; }
        public string LeagueName { get; set; }
        public string LeagueImage { get; set; }
        public bool? IsLive { get; set; }
        public bool? IsHot { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
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
            //var Match = _unitOfWork.Repository<Match>().Entities.FirstOrDefault(x => x.MessageName.Trim().ToLower().Equals(command.MessageName.Trim().ToLower()));
            //if (Match != null)
            //{
            //    return await Result<int>.FailureAsync($"Match đã tồn tại");
            //}
            var entity = _mapper.Map<Match>(command);
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
