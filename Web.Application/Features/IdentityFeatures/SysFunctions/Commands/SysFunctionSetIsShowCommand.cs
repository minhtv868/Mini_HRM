using Web.Application.Interfaces.Repositories;
using Web.Domain.Entities.Identity;
using Web.Shared;
using MediatR;

namespace Web.Application.Features.IdentityFeatures.SysFunctions.Commands
{
	public record SysFunctionSetIsShowCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
		public bool IsShow { get; set; }
	}

	internal class SysFunctionSetIsShowCommandHandler : IRequestHandler<SysFunctionSetIsShowCommand, Result<int>>
	{
		private readonly IIdentityUnitOfWork _unitOfWork;

		public SysFunctionSetIsShowCommandHandler(IIdentityUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<int>> Handle(SysFunctionSetIsShowCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Repository<SysFunction>().GetByIdAsync(command.Id);

			if (entity == null)
			{
				return await Result<int>.FailureAsync($"Chức năng Id <b>{command.Id}</b> không tồn tại.");
			}

			entity.IsShow = command.IsShow;

			await _unitOfWork.Repository<SysFunction>().UpdateAsync(entity.Id, entity);

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync($"Cập nhật trạng thái chức năng <b>{entity.FunctionName}</b> thành công.");
		}
	}
}
