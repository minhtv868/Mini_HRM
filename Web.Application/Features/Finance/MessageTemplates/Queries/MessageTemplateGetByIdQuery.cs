using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.MessageTemplates.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageTemplates.Queries
{
    public class MessageTemplateGetByIdQuery : IRequest<Result<MessageTemplateGetByIdDto>>
    {
        public short MessageTemplateId { get; set; }
    }
    internal class MessageTemplateGetByIdQueryHandler : IRequestHandler<MessageTemplateGetByIdQuery, Result<MessageTemplateGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MessageTemplateGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<MessageTemplateGetByIdDto>> Handle(MessageTemplateGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<MessageTemplate>().Entities.FirstOrDefault(x => x.MessageTemplateId == queryInput.MessageTemplateId);
            if (entity == null)
            {
                return await Result<MessageTemplateGetByIdDto>.FailureAsync("MessageTemplate không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<MessageTemplateGetByIdDto>(entity);
            return await Result<MessageTemplateGetByIdDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
