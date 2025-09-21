//using AutoMapper;
//using MediatR;
//using Web.Application.Common.Mappings;
//using Web.Application.Features.Finance.Matchs.DTOs;
//using Web.Application.Interfaces;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;
//using Web.Shared;

//namespace Web.Application.Features.Finance.Matchs.Commands
//{
//    public class MatchEditCommand : IRequest<Result<int>>, IMapFrom<Match>, IMapFrom<MatchGetByIdDto>
//    {
//        public int MatchId { get; set; }
//        public DateTime? EstimateStartTime { get; set; }
//        public short? HomeId { get; set; }
//        public short? AwayId { get; set; }
//        public short? LeagueId { get; set; }
//        public string HomeName { get; set; }
//        public string AwayName { get; set; }
//        public string HomeLogoPath { get; set; }
//        public string AwayLogoPath { get; set; }
//        public byte? HomeGoals { get; set; }
//        public byte? AwayGoals { get; set; }
//        public string StadiumName { get; set; }
//        public string LeagueName { get; set; }
//        public string LeagueImage { get; set; }
//        public bool? IsLive { get; set; }
//        public bool? IsHot { get; set; }
//        public int? SiteId { get; set; }
//        public int? CrUserId { get; set; }
//        public DateTime CrDateTime { get; set; }
//        public int? UpdUserId { get; set; }
//        public DateTime? UpdDateTime { get; set; }
//        public DateTime? LastUpdateTime { get; set; }
//    }
//    internal class MatchEditCommandHandler : IRequestHandler<MatchEditCommand, Result<int>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ICurrentUserService _currentUserService;
//        private readonly ISender _sender;
//        public MatchEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _currentUserService = currentUserService;
//            _sender = sender;
//        }
//        public async Task<Result<int>> Handle(MatchEditCommand command, CancellationToken cancellationToken)
//        {
//            //var entity = _unitOfWork.Repository<Match>().Entities.AsNoTracking().FirstOrDefault(x => x.MatchId == command.MatchId);
//            //if (entity == null)
//            //{
//            //    return await Result<int>.FailureAsync("Match không tồn tại");
//            //}
//            //if (command.EstimateStartTime != entity.MessageName)
//            //{
//            //    var existing = await _unitOfWork.Repository<Match>().Entities
//            // .Where(x => x.SiteId == command.SiteId)
//            // .AsNoTracking()
//            // .ToListAsync();
//            //    var existing2 = existing.FirstOrDefault(x => string.Equals(x.MessageName, command.MessageName, StringComparison.Ordinal));
//            //    if (existing2 != null)
//            //    {
//            //        return await Result<int>.FailureAsync("Match này đã tồn tại. Vui lòng chọn tên khác.");
//            //    }
//            //}
//            //entity = _mapper.Map<Match>(command);
//            //await _unitOfWork.Repository<Match>().UpdateFieldsAsync(entity,
//            //    x => x.SendFrom,
//            //    x => x.MessageName,
//            //    x => x.Title,
//            //    x => x.HomeGoals,
//            //    x => x.AwayGoals);
//            //var result = await _unitOfWork.Save(cancellationToken);
//            //if (result > 0)
//            //{
//            //    return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
//            //}
//            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
//        }
//    }
//}
