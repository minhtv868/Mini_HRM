using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Payroll : BaseAuditableEntity
    {
        public int PayrollId { get; set; }
        public int UserId { get; set; }
        public short Year { get; set; }
        public byte Month { get; set; }
        public byte? WorkingDays { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Deduction { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}