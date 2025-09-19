using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Sites.Commands
{
    public class SiteEditCommand : IRequest<Result<int>>, IMapFrom<Site>, IMapFrom<SiteGetByIdDto>
    {
        public short SiteId { get; set; }
        public string SiteName { get; set; }
        public string ShortName { get; set; }
        public string WebsiteDomain { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; } = true;
        public short? DisplayOrder { get; set; }
    }
    internal class SiteEditCommandHandler : IRequestHandler<SiteEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public SiteEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(SiteEditCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Site>();
            var entity = await repo.Entities.AsNoTracking()
                .FirstOrDefaultAsync(x => x.SiteId == command.SiteId, cancellationToken);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Site không tồn tại");
            }
            var duplicate = await repo.Entities.AsNoTracking()
                .AnyAsync(x => x.SiteId != command.SiteId
                            && x.SiteName.ToLower() == command.SiteName.ToLower(), cancellationToken);
            if (duplicate)
            {
                return await Result<int>.FailureAsync("Site này đã tồn tại. Vui lòng chọn tên khác.");
            }
            entity = _mapper.Map<Site>(command);
            entity.UpdUserId = _currentUserService.UserId;
            entity.UpdDateTime = DateTime.Now;
            await repo.UpdateFieldsAsync(entity,
                x => x.SiteName,
                x => x.ShortName,
                x => x.WebsiteDomain,
                x => x.Logo,
                x => x.IsActive,
                x => x.DisplayOrder);

            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
            }

            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }

    }
}
