using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Attendances.DTOs;
using Web.Application.Features.IdentityFeatures.Users.Queries;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Attendances.Queries
{
    public record AttendanceGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<AttendanceGetPageDto>>
    {
        [DisplayName("Site")]
        public int? SiteId { get; set; }
    }
    internal class AttendanceGetPageQueryHandler : IRequestHandler<AttendanceGetPageQuery, PaginatedResult<AttendanceGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public AttendanceGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<AttendanceGetPageDto>> Handle(AttendanceGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Attendance>().Entities;
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
            {
                //  query = query.Where(x => x.AttendanceName.Contains(queryInput.Keywords) || x.AttendanceDesc.Contains(queryInput.Keywords));
            }
            var result = await query.OrderByDescending(x => x.CrDateTime).ProjectTo<AttendanceGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            if (result.Data != null && result.Data.Any())
            {
                var userList = await _sender.Send(new UserGetAllQuery());
                foreach (var item in result.Data)
                {
                    if (item.CrUserId > 0)
                    {
                        var crUser = userList.Data.FirstOrDefault(x => x.Id == item.CrUserId);
                        item.UserName = crUser != null ? crUser.UserName : "...";
                    }
                }
            }
            await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}