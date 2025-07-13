using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Jobs;
using Web.Shared;
using MediatR;

namespace Web.Application.Features.Finances.Jobs.Commands
{
	public record JobDeleteCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }

	}
	internal class JobDeleteCommandHandler : IRequestHandler<JobDeleteCommand, Result<int>>
	{
		private readonly IFinanceUnitOfWork _unitOfWork;
		private readonly ICurrentUserService _currentUserService;

		public JobDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_currentUserService = currentUserService;
		}

		public async Task<Result<int>> Handle(JobDeleteCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Repository<Job>().GetByIdAsync(command.Id, false);

			if (entity == null)
			{
				return await Result<int>.FailureAsync($"Dữ liệu Id <b>{command.Id}</b> không tồn tại.");
			}

			var roles = await _currentUserService.GetRoles();


			bool isFullRole = _currentUserService.IsSuperAdminRole(roles);

			if (!isFullRole)
			{
				return await Result<int>.FailureAsync("Bạn không có quyền thực hiện thao tác này.");
			}

			await _unitOfWork.Repository<Job>().DeleteAsync(entity);

			var deletedResult = await _unitOfWork.Save(cancellationToken);

			if (deletedResult > 0)
			{

				return await Result<int>.SuccessAsync(entity.Id, $"Xóa dữ liệu thành công.");
			}
			return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công.");
		}
	}
}
