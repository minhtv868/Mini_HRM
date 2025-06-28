using Web.Application.DTOs;

namespace Web.Application.Interfaces
{
    public interface IAuditable
    {
        int? CrUserId { get; set; }
        int? UpdUserId { get; set; }
        int? CreatedBy { get; set; }
        int? UpdatedBy { get; set; }
        AuditableInfoDto AuditableInfo { get; set; }
    }
}
