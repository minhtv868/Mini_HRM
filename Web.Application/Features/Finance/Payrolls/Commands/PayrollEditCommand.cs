using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.Payrolls.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Payrolls.Commands
{
    public class PayrollEditCommand : IRequest<Result<int>>, IMapFrom<Payroll>, IMapFrom<PayrollGetByIdDto>
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
    }
    internal class PayrollEditCommandHandler : IRequestHandler<PayrollEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public PayrollEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService,
            ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(PayrollEditCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Payroll>().Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PayrollId == command.PayrollId, cancellationToken);

            if (entity == null)
            {
                return await Result<int>.FailureAsync("Payroll không tồn tại");
            }
            var entityAny = await _unitOfWork.Repository<Payroll>().Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == command.UserId
                                       && x.Month == command.Month
                                      && x.Year == command.Year
                                       && x.SiteId == command.SiteId
                                       && x.PayrollId != command.PayrollId, cancellationToken);

            if (entityAny != null)
            {
                return await Result<int>.FailureAsync("Bảng lương của nhân viên này trong tháng đã tồn tại.");
            }
            entity = _mapper.Map<Payroll>(command);
            entity.UpdDateTime = DateTime.Now;
            entity.UpdUserId = _currentUserService.UserId;
            await _unitOfWork.Repository<Payroll>().UpdateFieldsAsync(entity,
                x => x.UserId,
                x => x.Month,
                x => x.Year,
                x => x.WorkingDays,
                x => x.BasicSalary,
                x => x.Allowance,
                x => x.Bonus,
                x => x.Deduction,
                x => x.UpdUserId,
                x => x.UpdDateTime
            );

            var result = await _unitOfWork.Save(cancellationToken);

            if (result > 0)
            {
                return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
            }
            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }

    }
}