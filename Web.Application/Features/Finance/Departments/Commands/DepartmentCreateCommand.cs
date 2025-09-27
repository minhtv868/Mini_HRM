using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Departments.Commands
{
    public class DepartmentCreateCommand : IRequest<Result<int>>, IMapFrom<Department>
    {
        [DisplayName("Trưởng phòng")]
        public int? ManagerId { get; set; }

        [DisplayName("Tên")]
        public string DepartmentName { get; set; }

        [DisplayName("Mô tả")]
        public string DepartmentDesc { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class DepartmentCreateCommandHandler : IRequestHandler<DepartmentCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public DepartmentCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(DepartmentCreateCommand command, CancellationToken cancellationToken)
        {
            var entityAny = _unitOfWork.Repository<Department>().Entities.FirstOrDefault(x => x.DepartmentName.Trim().ToLower().Equals(command.DepartmentName.Trim().ToLower()));
            if (entityAny != null)
            {
                return await Result<int>.FailureAsync($"Department đã tồn tại");
            }
            var entity = _mapper.Map<Department>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Department>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}
