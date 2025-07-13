using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageTemplates.Commands
{
    public class MessageTemplateCreateCommand : IRequest<Result<int>>, IMapFrom<MessageTemplate>
    {
        public short MessageTemplateId { get; set; }
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
    internal class MessageTemplateCreateCommandHandler : IRequestHandler<MessageTemplateCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MessageTemplateCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MessageTemplateCreateCommand command, CancellationToken cancellationToken)
        {
            var MessageTemplate = _unitOfWork.Repository<MessageTemplate>().Entities.FirstOrDefault(x => x.MessageName.Trim().ToLower().Equals(command.MessageName.Trim().ToLower()));
            if (MessageTemplate != null)
            {
                return await Result<int>.FailureAsync($"MessageTemplate đã tồn tại");
            }
            var entity = _mapper.Map<MessageTemplate>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<MessageTemplate>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
