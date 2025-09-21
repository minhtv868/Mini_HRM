//using AutoMapper;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel;
//using Web.Application.Common.Mappings;
//using Web.Application.Features.Finance.Matchs.DTOs;
//using Web.Application.Interfaces;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;
//using Web.Shared;
//using Web.Shared.Helpers;

//namespace Web.Application.Features.Finance.Matchs.Commands
//{
//    public class MatchEditCommand : IRequest<Result<int>>, IMapFrom<Match>, IMapFrom<MatchGetByIdDto>
//    {
//        public int MatchId { get; set; }
//        [DisplayName("Thời gian")]
//        public DateTime EstimateStartTime { get; set; }
//        [DisplayName("Thời gian")]
//        public string EstimateStartTimeText { get; set; }

//        [DisplayName("Đội chủ nhà")]
//        public short? HomeId { get; set; }

//        [DisplayName("Đội khách")]
//        public short? AwayId { get; set; }

//        [DisplayName("Giải đấu")]
//        public short? LeagueId { get; set; }

//        [DisplayName("Tên đội chủ nhà")]
//        public string HomeName { get; set; }
//        [DisplayName("Tên đội khách")]
//        public string AwayName { get; set; }
//        [DisplayName("Logo đội chủ nhà")]
//        public string HomeLogoPath { get; set; }
//        [DisplayName("Logo đội khách")]
//        public string AwayLogoPath { get; set; }
//        [DisplayName("Bàn thắng đội chủ nhà")]
//        public byte? HomeGoals { get; set; }
//        [DisplayName("Bàn thắng đội khách")]
//        public byte? AwayGoals { get; set; }

//        [DisplayName("Sân vận động")]
//        public string StadiumName { get; set; }
//        [DisplayName("Tên giải đấu")]
//        public string LeagueName { get; set; }
//        [DisplayName("Logo giải đấu")]
//        public string LeagueImage { get; set; }
//        [DisplayName("Trực tiếp")]
//        public bool IsLive { get; set; }
//        [DisplayName("Nổi bật")]
//        public bool IsHot { get; set; }

//        [DisplayName("Site")]
//        public int? SiteId { get; set; }
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
//            try
//            {
//                var entity = _unitOfWork.Repository<Match>().Entities.AsNoTracking().FirstOrDefault(x => x.MatchId == command.MatchId);
//                if (entity == null)
//                {
//                    return await Result<int>.FailureAsync("Match không tồn tại");
//                }
//                //if (command.EstimateStartTime != entity.MessageName)
//                //{
//                //    var existing = await _unitOfWork.Repository<Match>().Entities
//                // .Where(x => x.SiteId == command.SiteId)
//                // .AsNoTracking()
//                // .ToListAsync();
//                //    var existing2 = existing.FirstOrDefault(x => string.Equals(x.MessageName, command.MessageName, StringComparison.Ordinal));
//                //    if (existing2 != null)
//                //    {
//                //        return await Result<int>.FailureAsync("Match này đã tồn tại. Vui lòng chọn tên khác.");
//                //    }
//                //}

//                entity = _mapper.Map<Match>(command);
//                if (!string.IsNullOrEmpty(command.EstimateStartTimeText))
//                {
//                    entity.EstimateStartTime = command.EstimateStartTimeText.StrToDateTime2("dd-MM-yyyy HH:mm");
//                }
//                entity.UpdUserId = _currentUserService.UserId;
//                entity.UpdDateTime = DateTime.Now;
//                await _unitOfWork.Repository<Match>().UpdateFieldsAsync(entity,
//                    x => x.EstimateStartTime,
//                    x => x.LeagueId,
//                    x => x.LeagueImage,
//                    x => x.HomeGoals,
//                    x => x.AwayGoals,
//                    x => x.StadiumName,
//                    x => x.HomeId,
//                    x => x.HomeName,
//                    x => x.HomeLogoPath,
//                    x => x.AwayId,
//                    x => x.AwayName,
//                    x => x.AwayLogoPath,
//                    x => x.IsHot,
//                    x => x.UpdUserId,
//                    x => x.UpdDateTime,
//                    x => x.LastUpdateTime);
//                var result = await _unitOfWork.Save(cancellationToken);
//                if (result > 0)
//                {
//                    return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
//                }
//                return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
//            }
//            catch (Exception e)
//            {
//                return await Result<int>.FailureAsync($"Cập nhật dữ liệu không thành công: {e.Message.ToString()}");
//            }

//        }
//    }
//}
