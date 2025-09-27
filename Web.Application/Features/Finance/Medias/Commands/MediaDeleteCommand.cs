using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Medias.Commands
{
    public class MediaDeleteCommand : IRequest<Result<int>>
    {
        public int MediaId { get; set; }
    }
    internal class MediaDeleteCommandHandler : IRequestHandler<MediaDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        public MediaDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(MediaDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Media>().Entities.AsNoTracking().FirstOrDefault(x => x.MediaId == command.MediaId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Media không tồn tại");
            }
            await _unitOfWork.Repository<Media>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}
