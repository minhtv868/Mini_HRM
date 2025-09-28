using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Payrolls.Commands
{
    public class PayrollCreateCommand : IRequest<Result<int>>, IMapFrom<Payroll>
    {
        public int PayrollId { get; set; }

        [DisplayName("Nhân viên")]
        public int UserId { get; set; }

        [DisplayName("Năm")]
        public short Year { get; set; }

        [DisplayName("Tháng")]
        public byte Month { get; set; }

        [DisplayName("Số ngày làm việc")]
        public byte? WorkingDays { get; set; }

        [DisplayName("Lương cơ bản")]
        public decimal BasicSalary { get; set; }

        [DisplayName("Phụ cấp")]
        public decimal? Allowance { get; set; }

        [DisplayName("Thưởng")]
        public decimal? Bonus { get; set; }

        [DisplayName("Khấu trừ")]
        public decimal? Deduction { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class PayrollCreateCommandHandler : IRequestHandler<PayrollCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public PayrollCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(PayrollCreateCommand command, CancellationToken cancellationToken)
        {
            var entityAny = _unitOfWork.Repository<Payroll>().Entities
                          .FirstOrDefault(x => x.UserId == command.UserId
                      && x.Month == command.Month
                      && x.Year == command.Year && x.SiteId == command.SiteId);

            if (entityAny != null)
            {
                return await Result<int>.FailureAsync($"Payroll đã tồn tại");
            }
            var entity = _mapper.Map<Payroll>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<Payroll>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}