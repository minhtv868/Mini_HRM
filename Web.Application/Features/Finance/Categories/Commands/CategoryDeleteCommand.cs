//using IC.Application.Interfaces;
//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienDocs;
//using IC.Application.PhapDienCMS.Categories.Helper;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Domain.Entities.PhapDienDocs;
//using IC.Shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using static Nest.JoinField;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//	public record CategoryDeleteCommand : IRequest<Result<int>>
//	{
//		public int CategoryId { get; set; }
//		public short SiteId { get; set; }
//	}

//	internal class CategoryDeleteCommandHandler : IRequestHandler<CategoryDeleteCommand, Result<int>>
//	{
//		private readonly IFinanceUnitOfWork _unitOfWork;
//		private readonly ICurrentUserService _currentUserService;
//		private readonly ICategoryRepo _categoryRepo;
//		public CategoryDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICategoryRepo categoryRepo)
//		{
//			_unitOfWork = unitOfWork;
//			_currentUserService = currentUserService;
//			_categoryRepo = categoryRepo;
//		}

//		public async Task<Result<int>> Handle(CategoryDeleteCommand command, CancellationToken cancellationToken)
//		{
//			var entity = await _unitOfWork.Repository<Category>().GetByIdAsync(command.CategoryId);
//			string message = string.Empty;
//			if (entity == null)
//			{
//				return await Result<int>.FailureAsync($"Id <b>{command.CategoryId}</b> không tồn tại.");
//			}
//			var query = _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//				.Where(x => x.SiteId == command.SiteId);
//			List<int> lisCategoryId = null;
//			var entitySort = query.Where(x => x.ParentCategoryId == entity.ParentCategoryId).FirstOrDefault();
//			if (command.CategoryId > 0)
//			{
//				var Categorys = await query.ToListAsync(cancellationToken);
//				lisCategoryId = SearchTree.SearchTreeOrder(Categorys, command.CategoryId);
//				query = query.Where(x => lisCategoryId.Contains(x.CategoryId));
//			}
//			var listCategorys = query.ToList();
//			var docCategorys = _unitOfWork.Repository<ArticleCategory>().Entities.Where(x => lisCategoryId.Contains(x.CategoryId));
//			if (docCategorys.Any())
//			{
//				var listDocUsed = docCategorys.Select(x => x.CategoryId).ToList();
//				var listNameUsed = query.Where(x => listDocUsed.Contains(x.CategoryId))
//										.Select(x => x.CategoryDesc)
//										.ToList();
//				var boldNames = listNameUsed.Select(name => $"<b>{name}</b>");
//				return await Result<int>.FailureAsync($"Xóa thất bại do chuyên mục {string.Join(", ", boldNames)} đã được sử dụng");
//			}
//			else
//			{
//				foreach (Category Category in listCategorys)
//				{
//					await _unitOfWork.Repository<Category>().DeleteAsync(Category);
//				}
//				var deleteChildResult = await _unitOfWork.Save(cancellationToken);
//				if (deleteChildResult > 0)
//				{
//					var listDeletedName = listCategorys.Select(x => x.CategoryDesc).ToList();
//					var boldNames = listDeletedName.Select(name => $"<b>{name}</b>");
//					if (entitySort != null)
//					{
//						await _categoryRepo.UpdateDisplayOrder(command.SiteId, entitySort.ParentCategoryId ?? 0, entitySort.DisplayOrder ?? 1, entitySort.CategoryId);
//						await _categoryRepo.UpdateTreeOrder(command.SiteId);
//					}
//					return await Result<int>.SuccessAsync($"Xóa thành công {string.Join(", ", boldNames)}.");
//				}
//			}
//			return await Result<int>.SuccessAsync("Xóa dữ liệu thành công.");
//		}

//		//public async Task<Result<int>> Handle(CategoryDeleteCommand command, CancellationToken cancellationToken)
//		//{
//		//	var entity = await _unitOfWork.Repository<Category>().GetByIdAsync(command.CategoryId);
//		//	string message = string.Empty;
//		//	if (entity == null)
//		//	{
//		//		return await Result<int>.FailureAsync($"Id <b>{command.CategoryId}</b> không tồn tại.");
//		//	}
//		//          var query = _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//		//		.Where(x => x.SiteId == command.SiteId);
//		//	List<int> lisCategoryId = null;
//		//	var entitySort = query.Where(x => x.ParentCategoryId == entity.ParentCategoryId).FirstOrDefault();
//		//	if (command.CategoryId > 0)
//		//	{
//		//		var categories = await query.ToListAsync(cancellationToken);
//		//		lisCategoryId = SearchTree.SearchTreeOrder(categories, command.CategoryId);
//		//		query = query.Where(x => lisCategoryId.Contains(x.CategoryId));
//		//	}
//		//	var listCategories = query.ToList();
//		//	foreach (Category Category in listCategories)
//		//	{
//		//		await _unitOfWork.Repository<Category>().DeleteAsync(Category);
//		//	}
//		//	var deleteChildResult = await _unitOfWork.Save(cancellationToken);
//		//	if (deleteChildResult > 0)
//		//	{
//		//		var listDeletedName = listCategories.Select(x => x.CategoryDesc).ToList();
//		//		var boldNames = listDeletedName.Select(name => $"<b>{name}</b>");
//		//		if (entitySort != null)
//		//		{
//		//			await _categoryRepo.UpdateDisplayOrder(command.SiteId, entitySort.ParentCategoryId ?? 0, entitySort.DisplayOrder ?? 1, entitySort.CategoryId);
//		//			await _categoryRepo.UpdateTreeOrder(command.SiteId);
//		//		}
//		//		return await Result<int>.SuccessAsync($"Xóa thành công {string.Join(", ", boldNames)}.");
//		//	}
//		//	return await Result<int>.SuccessAsync("Xóa dữ liệu thành công.");
//		//}
//	}
//}
