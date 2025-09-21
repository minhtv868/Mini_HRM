using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Matchs.Commands
{
    public class MatchCreateOrEditCommand : IRequest<Result<int>>, IMapFrom<Match>
    {
        public int MatchId { get; set; }
        public DateTime? EstimateStartTime { get; set; }
        public int? LSMatchId { get; set; }
        public string TimePlaying { get; set; }
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
        public byte? Status { get; set; }
        public bool? IsHot { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
    internal class MatchCreateOrEditCommandHandler : IRequestHandler<MatchCreateOrEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        private readonly ILogger<MatchCreateOrEditCommandHandler> _logger;
        public MatchCreateOrEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender, ILogger<MatchCreateOrEditCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
            _logger = logger;
        }
        public async Task<Result<int>> Handle(MatchCreateOrEditCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(command.LeagueName))
                {
                    var league = await _unitOfWork.Repository<League>().Entities
                        .FirstOrDefaultAsync(x => x.LeagueName == command.LeagueName, cancellationToken);

                    if (league != null)
                    {
                        command.LeagueId = league.LeagueId;
                        command.LeagueImage = league.LeagueImage ?? command.LeagueImage;
                    }
                }

                // 🔍 Tìm HomeTeam
                if (!string.IsNullOrEmpty(command.HomeName))
                {
                    var homeTeam = await _unitOfWork.Repository<Team>().Entities
                        .FirstOrDefaultAsync(x => x.TeamName == command.HomeName, cancellationToken);

                    if (homeTeam != null)
                    {
                        command.HomeId = homeTeam.TeamId;
                        command.HomeLogoPath = homeTeam.LogoPath ?? command.HomeLogoPath;
                    }
                }

                // 🔍 Tìm AwayTeam
                if (!string.IsNullOrEmpty(command.AwayName))
                {
                    var awayTeam = await _unitOfWork.Repository<Team>().Entities
                        .FirstOrDefaultAsync(x => x.TeamName == command.AwayName, cancellationToken);

                    if (awayTeam != null)
                    {
                        command.AwayId = awayTeam.TeamId;
                        command.AwayLogoPath = awayTeam.LogoPath ?? command.AwayLogoPath;
                    }
                }
                // Tìm match theo LSMatchId + EstimateStartTime + LeagueId
                var entity = await _unitOfWork.Repository<Match>().Entities
                    .FirstOrDefaultAsync(x =>
                        x.LSMatchId == command.LSMatchId &&
                        x.EstimateStartTime == command.EstimateStartTime &&
                        x.LeagueId == command.LeagueId,
                        cancellationToken);

                if (entity != null)
                {
                    // Update
                    entity.TimePlaying = command.TimePlaying;
                    entity.HomeId = command.HomeId;
                    entity.AwayId = command.AwayId;
                    entity.HomeName = command.HomeName;
                    entity.AwayName = command.AwayName;
                    entity.HomeLogoPath = command.HomeLogoPath;
                    entity.AwayLogoPath = command.AwayLogoPath;
                    entity.HomeGoals = command.HomeGoals;
                    entity.AwayGoals = command.AwayGoals;
                    entity.StadiumName = command.StadiumName;
                    entity.LeagueName = command.LeagueName;
                    entity.LeagueImage = command.LeagueImage;
                    entity.IsLive = command.IsLive;
                    entity.IsHot = command.IsHot;
                    entity.LastUpdateTime = DateTime.Now;

                    entity.UpdUserId = _currentUserService.UserId;
                    entity.UpdDateTime = DateTime.Now;

                    await _unitOfWork.Repository<Match>().UpdateFieldsAsync(entity, x => x.HomeGoals, x => x.AwayGoals);
                }
                else
                {
                    // Create mới
                    entity = new Match
                    {
                        EstimateStartTime = command.EstimateStartTime,
                        LSMatchId = command.LSMatchId,
                        TimePlaying = command.TimePlaying,
                        HomeId = command.HomeId,
                        AwayId = command.AwayId,
                        LeagueId = command.LeagueId,
                        HomeName = command.HomeName,
                        AwayName = command.AwayName,
                        HomeLogoPath = command.HomeLogoPath,
                        AwayLogoPath = command.AwayLogoPath,
                        HomeGoals = command.HomeGoals,
                        AwayGoals = command.AwayGoals,
                        StadiumName = command.StadiumName,
                        LeagueName = command.LeagueName,
                        LeagueImage = command.LeagueImage,
                        IsLive = command.IsLive,
                        Status = command.Status,
                        IsHot = command.IsHot,
                        LastUpdateTime = DateTime.Now,
                        CrUserId = _currentUserService.UserId,
                        CrDateTime = DateTime.Now
                    };

                    await _unitOfWork.Repository<Match>().AddAsync(entity);
                }

                var result = await _unitOfWork.Save(cancellationToken);

                if (result > 0)
                    return await Result<int>.SuccessAsync(entity.MatchId, "Lưu dữ liệu thành công");

                return await Result<int>.FailureAsync("Lưu dữ liệu không thành công");
            }
            catch (Exception e)
            {
                return await Result<int>.FailureAsync($"Lỗi: {e.Message}");
            }
        }

    }
}
