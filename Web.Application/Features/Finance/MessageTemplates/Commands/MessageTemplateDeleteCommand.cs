using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageTemplates.Commands
{
    public class MessageTemplateDeleteCommand : IRequest<Result<int>>
    {
        public short MessageTemplateId { get; set; }
    }
    internal class MessageTemplateDeleteCommandHandler : IRequestHandler<MessageTemplateDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public MessageTemplateDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(MessageTemplateDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<MessageTemplate>().Entities.AsNoTracking().FirstOrDefault(x => x.MessageTemplateId == command.MessageTemplateId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("MessageTemplate không tồn tại");
            }
            await _unitOfWork.Repository<MessageTemplate>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}
