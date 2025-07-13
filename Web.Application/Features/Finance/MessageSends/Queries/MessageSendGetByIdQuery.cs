using AutoMapper;
using MediatR;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageSends.Queries
{
    public class MessageSendGetByIdQuery : IRequest<Result<MessageSendGetByIdDto>>
    {
        public int MessageSendId { get; set; }
    }
    internal class MessageSendGetByIdQueryHandler : IRequestHandler<MessageSendGetByIdQuery, Result<MessageSendGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MessageSendGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public Task<Result<MessageSendGetByIdDto>> Handle(MessageSendGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<MessageSend>().Entities.FirstOrDefault(x => x.MessageSendId == queryInput.MessageSendId);
            if (entity == null)
            {
                return Result<MessageSendGetByIdDto>.FailureAsync("Tin nhắn không tồn tại");
            }
            var dataGetById = _mapper.Map<MessageSendGetByIdDto>(entity);
            return Result<MessageSendGetByIdDto>.SuccessAsync(dataGetById);
        }
    }
}
