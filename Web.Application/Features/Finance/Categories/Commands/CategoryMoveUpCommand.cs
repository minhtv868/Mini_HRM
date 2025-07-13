//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienDocs;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//	public class CategoryMoveUpCommand : IRequest<Result<int>>
//	{
//		public short CategoryId { get; set; }
//		public int SiteId { get; set; }
//	}

//	internal class CategoryMoveUpCommandHandler : IRequestHandler<CategoryMoveUpCommand, Result<int>>
//	{
//		private readonly IFinanceUnitOfWork _unitOfWork;
//		private readonly ICategoryRepo _CategoryRepo;

//		public CategoryMoveUpCommandHandler(IFinanceUnitOfWork unitOfWork, ICategoryRepo CategoryRepo)
//		{
//			_unitOfWork = unitOfWork;
//			_CategoryRepo = CategoryRepo;
//		}

//		public async Task<Result<int>> Handle(CategoryMoveUpCommand command, CancellationToken cancellationToken)
//		{
//			var entity = await _unitOfWork.Repository<Category>().Entities.FirstOrDefaultAsync(x => x.CategoryId == command.CategoryId);

//			if (entity == null)
//			{
//				return await Result<int>.FailureAsync($"Chuyên mục Id <b>{command.CategoryId}</b> không tồn tại.");
//			}
//			if (command.SiteId == 0)
//			{
//				command.SiteId = 18;
//			}

//			var entityAbove = await _unitOfWork.Repository<Category>().Entities
//								.Where(x => x.ParentCategoryId == entity.ParentCategoryId &&
//									   x.DisplayOrder < entity.DisplayOrder && x.SiteId == command.SiteId)
//								.OrderByDescending(o => o.DisplayOrder)
//								.FirstOrDefaultAsync();

//			if (entityAbove == null)
//			{
//				return await Result<int>.SuccessAsync($"Chuyên mục <b>{entity.CategoryDesc}</b> chỉ được phép di chuyển xuống dưới.");
//			}

//			short displayOrder = (short)entity.DisplayOrder;
//			entity.DisplayOrder = entityAbove.DisplayOrder;
//			entityAbove.DisplayOrder = displayOrder;

//			await _unitOfWork.Repository<Category>().UpdateAsync(entity.CategoryId, entity);

//			await _unitOfWork.Repository<Category>().UpdateAsync(entityAbove.CategoryId, entityAbove);

//			await _unitOfWork.Save(cancellationToken);

//			await _CategoryRepo.UpdateTreeOrder((short)command.SiteId);

//			return await Result<int>.SuccessAsync($"Di chuyển vị trí Chuyên mục <b>{entity.CategoryDesc}</b> thành công.");
//		}
//	}
//}
