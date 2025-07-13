//using FluentValidation;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using Microsoft.EntityFrameworkCore;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//    public class CategoryEditCommandValidator : AbstractValidator<CategoryEditCommand>
//	{
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        public CategoryEditCommandValidator(IFinanceUnitOfWork unitOfWork)
//		{
//            _unitOfWork = unitOfWork;
//            RuleFor(x => x.CategoryName)
//				.NotEmpty()
//				.WithMessage("Tên không được để trống")
//				.MaximumLength(250)
//				.WithMessage("Tên không được quá 200 ký tự")
//                .MustAsync(IsUniqueName)
//                .WithMessage(x => $"Tên chuyên mục <b>{x.CategoryName}</b> đã được sử dụng.");
//            RuleFor(x => x.CategoryDesc)
//				.NotEmpty()
//				.WithMessage("Miêu tả không được để trống")
//				.MaximumLength(250)
//				.WithMessage("Miêu tả không được quá 200 ký tự");
//			RuleFor(x => x.ReviewStatusId)
//			   .NotEmpty()
//			   .WithMessage("Miêu tả không được để trống");
//		}
//        private async Task<bool> IsUniqueName(CategoryEditCommand instance, string name, CancellationToken cancellationToken)
//        {
//            var isExisting = await _unitOfWork.Repository<Category>().Entities.AsNoTracking()
//                .FirstOrDefaultAsync(x => x.CategoryName.ToLower().Trim().Equals(name.ToLower().Trim()) && x.CategoryId != instance.CategoryId && x.SiteId == instance.SiteId, cancellationToken);

//            return isExisting == null;
//        }
//    }
//}
