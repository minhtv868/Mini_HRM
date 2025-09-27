using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.Departments.DTOs
{
    public class DepartmentGetPageDto : DepartmentDto, IAuditable
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public AuditableInfoDto AuditableInfo { get; set; }
    }
}
