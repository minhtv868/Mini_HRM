using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Articles.Commands
{
    public class ArticleCreateCommand : IRequest<Result<int>>, IMapFrom<Article>
    {
        public short ArticleId { get; set; }
        [DisplayName("Site")]
        public short? SiteId { get; set; }
        [DisplayName("Tên")]
        public string MessageName { get; set; }
        [DisplayName("Gửi từ")]
        public string SendFrom { get; set; }
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [DisplayName("Nội dung")]
        public string MsgContent { get; set; }
        [DisplayName("Hình thức gửi")]
        public byte SendMethodId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class ArticleCreateCommandHandler : IRequestHandler<ArticleCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public ArticleCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(ArticleCreateCommand command, CancellationToken cancellationToken)
        {
            var Article = _unitOfWork.Repository<Article>().Entities.FirstOrDefault(x => x.Title.Trim().ToLower().Equals(command.MessageName.Trim().ToLower()));
            if (Article != null)
            {
                return await Result<int>.FailureAsync($"Article đã tồn tại");
            }
            var entity = _mapper.Map<Article>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Article>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
