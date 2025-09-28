using AutoMapper;
using MediatR;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.IdentityFeatures.Users.Commands
{
    public class UserAssignSitesCommand : IRequest<Result<int>>, IMapFrom<UserSite>
    {
        public int UserId { get; set; }
        public List<short> SelectedSiteIds { get; set; }
    }
    internal class UserAssignSitesCommandHandler : IRequestHandler<UserAssignSitesCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public UserAssignSitesCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(UserAssignSitesCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.UserId == 0)
                {
                    return await Result<int>.FailureAsync("Người dùng không tồn tại");
                }
                if (command.SelectedSiteIds != null && command.SelectedSiteIds.Any())
                {
                    var userSites = _unitOfWork.Repository<UserSite>().Entities.Where(x => x.UserId == command.UserId).ToList();
                    if (userSites != null && userSites.Any())
                    {
                        await _unitOfWork.Repository<UserSite>().DeleteManyAsync(userSites);

                        await _unitOfWork.Save(cancellationToken);
                    }
                    foreach (var siteId in command.SelectedSiteIds)
                    {
                        var entity = _mapper.Map<UserSite>(command);
                        entity.SiteId = siteId;
                        entity.CrDateTime = DateTime.Now;
                        entity.CrUserId = _currentUserService.UserId;
                        await _unitOfWork.Repository<UserSite>().AddAsync(entity);
                        var insertresult = await _unitOfWork.Save(cancellationToken);
                    }
                }
                return await Result<int>.SuccessAsync("Gán site thành công");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
