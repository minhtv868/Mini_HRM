//using AutoMapper;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using IC.Application.Common.Mappings;
//using IC.Application.Features.PhapDienCMS.Sites.Queries;
//using IC.Application.Interfaces;
//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienDocs;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Domain.Entities.PhapDienDocs;
//using IC.Domain.Enums.PhapDienDocs;
//using IC.Shared;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//	public class CategoryCreateCommand : IRequest<Result<int>>, IMapFrom<Category>
//	{
//		public List<SiteGetAllByUserDto> SiteList;
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

//		[DisplayName("Thêm tiếp dữ liệu khác")]
//		public bool AddMoreData { get; set; }
//		public byte CategoryLevel { get; set; }
//		[DisplayName("Vị trí sắp xếp")]
//		public byte ItemPosition { get; set; }
//		[DisplayName("Sau mục")]
//		public int AfterCategoryId { get; set; }

//	}
//	internal class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, Result<int>>
//	{
//		private readonly IFinanceUnitOfWork _unitOfWork;
//		private readonly IMapper _mapper;
//		private readonly ICurrentUserService _currentUserService;
//		private readonly ICategoryRepo _categoryRepo;
//		public CategoryCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ICategoryRepo categoryRepo)
//		{
//			_unitOfWork = unitOfWork;
//			_mapper = mapper;
//			_currentUserService = currentUserService;
//			_categoryRepo = categoryRepo;
//		}
//		public async Task<Result<int>> Handle(CategoryCreateCommand command, CancellationToken cancellationToken)
//		{
//			if (command.SiteId == 0)
//			{
//				command.SiteId = (short)SiteIdEnum.Default;
//			}

//			var roles = await _currentUserService.GetRoles();

//			var isAdmin = _currentUserService.IsAdminRole(roles);

//			if (!isAdmin)
//			{
//				var hasPermissionWithSite = await _currentUserService.HasPermissionWithSite(command.SiteId);
//				if (!hasPermissionWithSite)
//				{
//					return await Result<int>.FailureAsync($"Bạn không có quyền thêm cho site này ");
//				}
//			}

//			//byte categoryLevel = 0; // Default or fallback value
//			//if (command.ParentCategoryId > 0)
//			//{
//			//	var parent = await _unitOfWork.Repository<Category>().GetByIdAsync((short)command.ParentCategoryId);
//			//	command.CategoryLevel = (byte)(parent.CategoryLevel + 1);
//			//}
//			//else
//			//{
//			//	command.CategoryLevel = categoryLevel;
//			//}

//			var entity = _mapper.Map<Category>(command);
//			entity.CrDateTime = DateTime.Now;
//			entity.CrUserId = _currentUserService.UserId;
//			entity.ShowTop = command.Top ? (byte)1 : (byte)0;
//			entity.ShowBottom = command.Bottom ? (byte)1 : (byte)0;
//			entity.ShowWeb = command.Web ? (byte)1 : (byte)0;
//			entity.ShowWap = command.Wap ? (byte)1 : (byte)0;
//			entity.ShowApp = command.App ? (byte)1 : (byte)0;
//			//entity.DisplayOrder = (byte?)((await _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//			//	.Where(x => x.ParentCategoryId == command.ParentCategoryId)
//			//	.MaxAsync(x => (byte?)x.DisplayOrder) ?? 0) + 1);
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
//				entity.DisplayOrder = categoryBefore.DisplayOrder + 1;
//				entity.CategoryLevel = categoryBefore.CategoryLevel;
//			}
//			await _unitOfWork.Repository<Category>().AddAsync(entity);
//			var result = await _unitOfWork.Save(cancellationToken);

//			if (result > 0)
//			{
//				await _categoryRepo.UpdateDisplayOrder(command.SiteId, command.ParentCategoryId, entity.DisplayOrder ?? 1, entity.CategoryId);
//				await _categoryRepo.UpdateTreeOrder(command.SiteId);
//				return await Result<int>.SuccessAsync(entity.CategoryId, "Thêm dữ liệu thành công.");
//			}

//			return await Result<int>.FailureAsync("Thêm dữ liệu không thành công.");

//		}
//	}
//}
