using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.InterestRates.DTOs
{
    public class InterestRateGetAllBySiteDto : IMapFrom<InterestRate>
    {
        public int RateId { get; set; }
        public int BankId { get; set; }
        public int TermMonths { get; set; }
        public string RateType { get; set; } = "Tại quầy";
        public decimal InterestRateValue { get; set; }
        public string Currency { get; set; } = "VND";
        public DateTime EffectiveDate { get; set; }
        public string? Note { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
