using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Sites.Commands
{
    public class SiteCreateCommand : IRequest<Result<int>>, IMapFrom<Site>
    {
        public short SiteId { get; set; }

        [DisplayName("Tên site")]
        public string SiteName { get; set; }

        [DisplayName("Tên site ngắn")]
        public string ShortName { get; set; }

        [DisplayName("Link web")]
        public string WebsiteDomain { get; set; }

        [DisplayName("Ảnh")]
        public string Logo { get; set; }

        [DisplayName("Hoạt động")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Số thứ tự")]
        public short? DisplayOrder { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class SiteCreateCommandHandler : IRequestHandler<SiteCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public SiteCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(SiteCreateCommand command, CancellationToken cancellationToken)
        {
            var entityAny = _unitOfWork.Repository<Site>().Entities.FirstOrDefault(x => x.SiteName.Trim().ToLower().Equals(command.SiteName.Trim().ToLower()));
            if (entityAny != null)
            {
                return await Result<int>.FailureAsync($"Site đã tồn tại");
            }
            var entity = _mapper.Map<Site>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Site>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
