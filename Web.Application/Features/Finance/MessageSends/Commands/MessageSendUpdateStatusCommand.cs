using MediatR;
using Microsoft.Extensions.Logging;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.MessageSends.Commands
{
    public class MessageSendUpdateStatusCommand : IRequest<Result<int>>, IMapFrom<MessageSend>
    {
        public int MessageSendId { get; set; }
        public byte SendStatusId { get; set; }
    }
    internal class MessageSendUpdateStatusCommandHandler : IRequestHandler<MessageSendUpdateStatusCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        ILogger<MessageSendUpdateStatusCommand> _logger;
        public MessageSendUpdateStatusCommandHandler(IFinanceUnitOfWork unitOfWork, IMediator mediator, ILogger<MessageSendUpdateStatusCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }
        public async Task<Result<int>> Handle(MessageSendUpdateStatusCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _unitOfWork.Repository<MessageSend>().GetByIdAsync(command.MessageSendId);
                if (entity == null)
                {
                    return await Result<int>.FailureAsync("MessageSend không tồn tại");
                }

                string sqlUpdate = $"UPDATE MessageSends SET SendStatusId = {command.SendStatusId} WHERE MessageSendId = {entity.MessageSendId}";
                await _unitOfWork.Repository<MessageSend>().ExecNoneQuerySql(sqlUpdate);
                return await Result<int>.SuccessAsync(command.MessageSendId, "Cập nhật dữ liệu thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }
    }
}
