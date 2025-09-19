using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Sites.Commands
{
    public class SiteDeleteCommand : IRequest<Result<int>>
    {
        public short SiteId { get; set; }
    }
    internal class SiteDeleteCommandHandler : IRequestHandler<SiteDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        public SiteDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(SiteDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Site>().Entities.AsNoTracking().FirstOrDefault(x => x.SiteId == command.SiteId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Site không tồn tại");
            }
            await _unitOfWork.Repository<Site>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}
