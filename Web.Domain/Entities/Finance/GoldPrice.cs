using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Entities.Finance
{
    public class GoldPrice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public string Source { get; set; }
        [StringLength(100)]
        public string Brand { get; set; }

        [StringLength(100)]
        public string Location { get; set; }
        public DateTime CrDateTime { get; set; }
        public DateTime UpdDateTime { get; set; }

    }
}
