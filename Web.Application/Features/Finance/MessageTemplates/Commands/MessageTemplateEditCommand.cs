using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.MessageTemplates.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageTemplates.Commands
{
    public class MessageTemplateEditCommand : IRequest<Result<int>>, IMapFrom<MessageTemplate>, IMapFrom<MessageTemplateGetByIdDto>
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
    }
    internal class MessageTemplateEditCommandHandler : IRequestHandler<MessageTemplateEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MessageTemplateEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MessageTemplateEditCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<MessageTemplate>().Entities.AsNoTracking().FirstOrDefault(x => x.MessageTemplateId == command.MessageTemplateId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("MessageTemplate không tồn tại");
            }
            if (command.MessageName != entity.MessageName)
            {
                var existing = await _unitOfWork.Repository<MessageTemplate>().Entities
             .Where(x => x.SiteId == command.SiteId)
             .AsNoTracking()
             .ToListAsync();
                var existing2 = existing.FirstOrDefault(x => string.Equals(x.MessageName, command.MessageName, StringComparison.Ordinal));
                if (existing2 != null)
                {
                    return await Result<int>.FailureAsync("MessageTemplate này đã tồn tại. Vui lòng chọn tên khác.");
                }
            }
            entity = _mapper.Map<MessageTemplate>(command);
            await _unitOfWork.Repository<MessageTemplate>().UpdateFieldsAsync(entity,
                x => x.SendFrom,
                x => x.MessageName,
                x => x.Title,
                x => x.MsgContent,
                x => x.SendMethodId);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
            }
            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }
    }
}
