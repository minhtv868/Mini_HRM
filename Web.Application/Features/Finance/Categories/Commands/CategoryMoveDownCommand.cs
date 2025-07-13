//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienDocs;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//	public class CategoryMoveDownCommand : IRequest<Result<int>>
//	{
//		public short CategoryId { get; set; }
//		public int SiteId { get; set; }
//	}

//	internal class CategoryMoveDownCommandHandler : IRequestHandler<CategoryMoveDownCommand, Result<int>>
//	{
//		private readonly IFinanceUnitOfWork _unitOfWork;
//		private readonly ICategoryRepo _CategoryRepo;

//		public CategoryMoveDownCommandHandler(IFinanceUnitOfWork unitOfWork, ICategoryRepo CategoryRepo)
//		{
//			_unitOfWork = unitOfWork;
//			_CategoryRepo = CategoryRepo;
//		}

//		public async Task<Result<int>> Handle(CategoryMoveDownCommand command, CancellationToken cancellationToken)
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

//			var entityBelow = await _unitOfWork.Repository<Category>().Entities
//								.Where(x => x.ParentCategoryId == entity.ParentCategoryId && x.DisplayOrder > entity.DisplayOrder && x.SiteId == command.SiteId)
//								.OrderBy(o => o.DisplayOrder)
//								.FirstOrDefaultAsync();

//			if (entityBelow == null)
//			{
//				return await Result<int>.SuccessAsync($"Chuyên mục <b>{entity.CategoryDesc}</b> chỉ được phép di chuyển lên trên.");
//			}

//			short displayOrder = (short)entity.DisplayOrder;
//			entity.DisplayOrder = entityBelow.DisplayOrder;
//			entityBelow.DisplayOrder = displayOrder;

//			await _unitOfWork.Repository<Category>().UpdateAsync(entity.CategoryId, entity);

//			await _unitOfWork.Repository<Category>().UpdateAsync(entityBelow.CategoryId, entityBelow);

//			await _unitOfWork.Save(cancellationToken);

//			await _CategoryRepo.UpdateTreeOrder((short)command.SiteId);

//			return await Result<int>.SuccessAsync($"Di chuyển vị trí chuyên mục <b>{entity.CategoryDesc}</b> thành công.");
//		}
//	}
//}
