using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.Attendances.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Attendances.Commands
{
    public class AttendanceEditCommand : IRequest<Result<int>>, IMapFrom<Attendance>, IMapFrom<AttendanceGetByIdDto>
    {
        public int AttendanceId { get; set; }
        [DisplayName("Trưởng phòng")]
        public int? ManagerId { get; set; }

        [DisplayName("Tên")]
        public string AttendanceName { get; set; }

        [DisplayName("Mô tả")]
        public string AttendanceDesc { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }
    }
    internal class AttendanceEditCommandHandler : IRequestHandler<AttendanceEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public AttendanceEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService,
            ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(AttendanceEditCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Attendance>().Entities.AsNoTracking().FirstOrDefault(x => x.AttendanceId == command.AttendanceId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Attendance không tồn tại");
            }
            //if (command.AttendanceName != entity.AttendanceName)
            //{
            //    var existing = await _unitOfWork.Repository<Attendance>().Entities
            // .Where(x => x.SiteId == command.SiteId)
            // .AsNoTracking()
            // .ToListAsync();
            //    var existing2 = existing.FirstOrDefault(x => string.Equals(x.AttendanceName, command.AttendanceName, StringComparison.Ordinal));
            //    if (existing2 != null)
            //    {
            //        return await Result<int>.FailureAsync("Attendance này đã tồn tại. Vui lòng chọn tên khác.");
            //    }
            //}
            entity = _mapper.Map<Attendance>(command);
            entity.UpdDateTime = DateTime.Now;
            entity.UpdUserId = _currentUserService.UserId;
            await _unitOfWork.Repository<Attendance>().UpdateFieldsAsync(entity,


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