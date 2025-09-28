using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Payrolls.DTOs
{
    public class PayrollDto : IMapFrom<Payroll>
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