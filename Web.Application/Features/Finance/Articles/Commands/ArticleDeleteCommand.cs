using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Articles.Commands
{
    public class ArticleDeleteCommand : IRequest<Result<int>>
    {
        public short ArticleId { get; set; }
    }
    internal class ArticleDeleteCommandHandler : IRequestHandler<ArticleDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public ArticleDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(ArticleDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Article>().Entities.AsNoTracking().FirstOrDefault(x => x.Id == command.ArticleId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Article không tồn tại");
            }
            await _unitOfWork.Repository<Article>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}
