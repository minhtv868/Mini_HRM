//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;
//using Web.Shared;

//namespace Web.Application.Features.Finance.Matchs.Commands
//{
//    public class MatchDeleteCommand : IRequest<Result<int>>
//    {
//        public short MatchId { get; set; }
//    }
//    internal class MatchDeleteCommandHandler : IRequestHandler<MatchDeleteCommand, Result<int>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly ISender _sender;
//        public MatchDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, ISender sender)
//        {
//            _unitOfWork = unitOfWork;
//            _sender = sender;
//        }
//        public async Task<Result<int>> Handle(MatchDeleteCommand command, CancellationToken cancellationToken)
//        {
//            var entity = _unitOfWork.Repository<Match>().Entities.AsNoTracking().FirstOrDefault(x => x.MatchId == command.MatchId);
//            if (entity == null)
//            {
//                return await Result<int>.FailureAsync("Match không tồn tại");
//            }
//            await _unitOfWork.Repository<Match>().DeleteAsync(entity);

//            var deleteResult = await _unitOfWork.Save(cancellationToken);
//            if (deleteResult > 0)
//            {
//                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
//            }

//            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
//        }
//    }
//}
