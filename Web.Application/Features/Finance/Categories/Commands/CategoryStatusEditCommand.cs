//using AutoMapper;
//using IC.Application.Common.Mappings;
//using IC.Application.Interfaces;
//using IC.Application.Interfaces.Repositories.PhapDienCMS;
//using IC.Domain.Entities.PhapDienCMS;
//using IC.Domain.Enums.PhapDienCMS;
//using IC.Shared;
//using MediatR;
//using System.ComponentModel;

//namespace IC.Application.Features.PhapDienCMS.Categories.Commands
//{
//    public record CategoryStatusEditCommand : IRequest<Result<int>>, IMapFrom<Category>
//    {
//        public int CategoryId { get; set; }
//        [DisplayName("Hiển thị")]
//        public byte Status { get; set; }
//        [DisplayName("Site")]
//        public short? SiteId { get; set; }
//    }

//    internal class CategoryStatusEditCommandHandler : IRequestHandler<CategoryStatusEditCommand, Result<int>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ICurrentUserService _currentUserService;
//        public CategoryStatusEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _currentUserService = currentUserService;
//        }

//        public async Task<Result<int>> Handle(CategoryStatusEditCommand command, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var entity = await _unitOfWork.Repository<Category>().GetByIdAsync(command.CategoryId);

//                if (entity == null)
//                {
//                    return await Result<int>.FailureAsync($"Id <b>{command.CategoryId}</b> không tồn tại ");
//                }

//                var roles = await _currentUserService.GetRoles();

//                var isAdmin = _currentUserService.IsAdminRole(roles);

//                if (!isAdmin)
//                {
//                    var hasPermissionWithSite = await _currentUserService.HasPermissionWithSite(command.SiteId.Value);
//                    if (!hasPermissionWithSite)
//                    {
//                        return await Result<int>.FailureAsync($"Bạn không có quyền sửa đơn vị soạn thảo cho site này ");
//                    }

//                }
//                entity = _mapper.Map<Category>(command);
//                entity.ReviewStatusId = command.Status;
//                await _unitOfWork.Repository<Category>().UpdateFieldsAsync(entity,
//                    x => x.ReviewStatusId);

//                var result = await _unitOfWork.Save(cancellationToken);

//                return result > 0
//                        ? await Result<int>.SuccessAsync(entity.CategoryId, "Thay đổi trạng thái chuyên mục thành công.")
//                        : await Result<int>.FailureAsync("Thay đổi trạng thái chuyên mục không thành công.");
//            }
//            catch (Exception e)
//            {

//                throw;
//            }
//        }
//    }
//}
