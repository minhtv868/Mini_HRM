using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.UserSites.Queries
{
    public class UserSiteGetByUserIdQuery : IRequest<List<UserSiteGetByUserIdDto>>
    {
        public int UserId { get; set; }
    }
    internal class UserSiteGetByUserIdQueryHandler : IRequestHandler<UserSiteGetByUserIdQuery, List<UserSiteGetByUserIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public UserSiteGetByUserIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<List<UserSiteGetByUserIdDto>> Handle(UserSiteGetByUserIdQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<UserSite>().Entities;
            if (queryInput.UserId > 0)
            {
                query = query.Where(x => x.UserId == queryInput.UserId);
            }
            var result = await query.ProjectTo<UserSiteGetByUserIdDto>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }
    }
}
