using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Departments.Commands
{
    public class DepartmentDeleteCommand : IRequest<Result<int>>
    {
        public int DepartmentId { get; set; }
    }
    internal class DepartmentDeleteCommandHandler : IRequestHandler<DepartmentDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        public DepartmentDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DepartmentDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Department>().Entities.AsNoTracking().FirstOrDefault(x => x.DepartmentId == command.DepartmentId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Department không tồn tại");
            }
            await _unitOfWork.Repository<Department>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}
