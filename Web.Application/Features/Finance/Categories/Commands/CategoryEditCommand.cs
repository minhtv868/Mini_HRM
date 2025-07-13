//using AutoMapper;
//using IC.Application.Common.Mappings;
//using IC.Application.Features.PhapDienCMS.Categories.DTOs;
//using IC.Application.Features.PhapDienCMS.Sites.Queries;
//using IC.Application.Interfaces;
//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienDocs;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Application.PhapDienCMS.Categories.Helper;
//using IC.Shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//	public record CategoryEditCommand : IRequest<Result<int>>, IMapFrom<Category>, IMapFrom<CategoryGetByIdDto>
//	{
//		public List<SiteGetAllByUserDto> SiteList;
//		public int CategoryId { get; set; }
//		[DisplayName("Site")]
//		public short SiteId { get; set; }

//		[DisplayName("Loại dữ liệu")]
//		public short? DataTypeId { get; set; } = 1;

//		[DisplayName("Chuyên mục cha")]
//		public int ParentCategoryId { get; set; }

//		[DisplayName("Tên chuyên mục ")]
//		public string CategoryName { get; set; }

//		[DisplayName("Mô tả")]
//		public string CategoryDesc { get; set; }

//		[DisplayName("Nhóm thuộc tính")]
//		public short? FeatureGroupId { get; set; }

//		[DisplayName("Thứ tự hiển thị")]
//		public int? DisplayOrder { get; set; }

//		[DisplayName("Kiểu URL Rewrite")]
//		public string UrlRewriteType { get; set; }

//		[DisplayName("URL")]
//		public string CategoryUrl { get; set; }

//		[DisplayName("Tiêu đề SEO")]
//		public string MetaTitle { get; set; }

//		[DisplayName("Mô tả SEO")]
//		public string MetaDesc { get; set; }

//		[DisplayName("Từ khóa SEO")]
//		public string MetaKeyword { get; set; }

//		[DisplayName("Thẻ Canonical")]
//		public string CanonicalTag { get; set; }

//		[DisplayName("Nội dung thẻ H1")]
//		public string H1Tag { get; set; }

//		[DisplayName("SEO Footer")]
//		public string SeoFooter { get; set; }

//		[DisplayName("Hiển thị Top")]
//		public bool Top { get; set; }
//		[DisplayName("Hiển thị Bottom")]
//		public bool Bottom { get; set; }
//		[DisplayName("Hiển thị Web")]
//		public bool Web { get; set; }
//		[DisplayName("Hiển thị Wap")]
//		public bool Wap { get; set; }
//		[DisplayName("Hiển thị App")]
//		public bool App { get; set; }

//		[DisplayName("Icon")]
//		public string ImagePath { get; set; }

//		[DisplayName("Trạng thái")]
//		public byte ReviewStatusId { get; set; }
//		public byte CategoryLevel { get; set; }
//		public int CurrentParentCategoryId { get; set; }
//		[DisplayName("Sau mục")]
//		public int AfterCategoryId { get; set; }
//		[DisplayName("Vị trí sắp xếp")]
//		public byte ItemPosition { get; set; }
//	}

//	internal class CategoryEditCommandHandler : IRequestHandler<CategoryEditCommand, Result<int>>
//	{
//		private readonly IFinanceUnitOfWork _unitOfWork;
//		private readonly IMapper _mapper;
//		private readonly ICurrentUserService _currentUserService;
//		private readonly ICategoryRepo _CategoryRepo;
//		public CategoryEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ICategoryRepo CategoryRepo)
//		{
//			_unitOfWork = unitOfWork;
//			_mapper = mapper;
//			_currentUserService = currentUserService;
//			_CategoryRepo = CategoryRepo;
//		}

//		public async Task<Result<int>> Handle(CategoryEditCommand command, CancellationToken cancellationToken)
//		{
//			int currentDisplayOrder = 0;
//			var entity = await _unitOfWork.Repository<Category>().GetByIdAsync(command.CategoryId);
//			var categorys = _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//				.Where(x => x.ParentCategoryId == command.ParentCategoryId && x.SiteId == command.SiteId);

//			if (entity == null)
//			{
//				return await Result<int>.FailureAsync($"Id <b>{command.CategoryId}</b> không tồn tại ");
//			}
//			if (command.ParentCategoryId > 0 && entity.ParentCategoryId != command.ParentCategoryId)
//			{
//				var query = _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//					.Where(x => x.SiteId == command.SiteId);
//				var childOfCategory = await query.ToListAsync(cancellationToken);
//				var lisCategoryId = SearchTree.SearchTreeOrder(childOfCategory, command.CategoryId);
//				if (lisCategoryId != null && lisCategoryId.Contains(command.ParentCategoryId))
//				{
//					return await Result<int>.FailureAsync($"Chuyên mục cha nằm trong <b>{entity.CategoryName}</b>.");
//				}
//			}
//			byte CategoryLevel = 0;
//			if (command.ParentCategoryId > 0)
//			{
//				var parent = await _unitOfWork.Repository<Category>().GetByIdAsync(command.ParentCategoryId);
//				command.CategoryLevel = (byte)(parent.CategoryLevel + 1);
//			}
//			else
//			{
//				command.CategoryLevel = CategoryLevel;
//			}

//			entity = _mapper.Map<Category>(command);
//			//entity.DisplayOrder = command.DisplayOrder;
//			entity.ReviewStatusId = command.ReviewStatusId;
//			entity.ShowTop = command.Top ? (byte)1 : (byte)0;
//			entity.ShowBottom = command.Bottom ? (byte)1 : (byte)0;
//			entity.ShowWeb = command.Web ? (byte)1 : (byte)0;
//			entity.ShowWap = command.Wap ? (byte)1 : (byte)0;
//			entity.ShowApp = command.App ? (byte)1 : (byte)0;
//			if (command.CurrentParentCategoryId != entity.ParentCategoryId)
//			{
//				entity.DisplayOrder = (await categorys.MaxAsync(x => x.DisplayOrder) ?? 0) + 1;
//			}
//			if (command.ItemPosition == 1)
//			{
//				entity.DisplayOrder = (await _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//				.Where(x => x.ParentCategoryId == command.ParentCategoryId)
//				.MaxAsync(x => x.DisplayOrder) ?? 0) + 1;
//			}
//			else if (command.ItemPosition == 2)
//			{
//				entity.DisplayOrder = 1;
//			}
//			else if (command.ItemPosition == 3 && command.AfterCategoryId > 0)
//			{
//				var categoryBefore = await _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//					.FirstOrDefaultAsync(x => x.CategoryId == command.AfterCategoryId);
//				if (entity.DisplayOrder < categoryBefore.DisplayOrder)
//				{
//					entity.DisplayOrder = (byte?)(categoryBefore.DisplayOrder);
//				}
//				else
//				{
//					entity.DisplayOrder = (byte?)(categoryBefore.DisplayOrder + 1);
//				}
//				entity.CategoryLevel = categoryBefore.CategoryLevel;
//			}
//			await _unitOfWork.Repository<Category>().UpdateFieldsAsync(entity,
//				x => x.DataTypeId,
//				x => x.ParentCategoryId,
//				x => x.CategoryName,
//				x => x.CategoryDesc,
//				x => x.FeatureGroupId,
//				x => x.DisplayOrder,
//				x => x.UrlRewriteType,
//				x => x.CategoryUrl,
//				x => x.MetaTitle,
//				x => x.MetaDesc,
//				x => x.MetaKeyword,
//				x => x.CanonicalTag,
//				x => x.H1Tag,
//				x => x.SeoFooter,
//				x => x.ShowTop,
//				x => x.ShowBottom,
//				x => x.ShowWeb,
//				x => x.ShowWap,
//				x => x.ShowApp,
//				x => x.ImagePath,
//				x => x.ReviewStatusId);

//			await _unitOfWork.Save(cancellationToken);
//			var updateResult = await _unitOfWork.Save(cancellationToken);
//			//if (updateResult > 0)
//			//{
//			await _CategoryRepo.UpdateDisplayOrder(command.SiteId, command.ParentCategoryId, entity.DisplayOrder ?? 1, entity.CategoryId);
//			await _CategoryRepo.UpdateTreeOrder(entity.SiteId ?? 18);
//			//}

//			return await Result<int>.SuccessAsync(entity.CategoryId, "Cập nhật thành công.");
//		}

//	}
//}
