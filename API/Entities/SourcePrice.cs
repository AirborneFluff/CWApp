using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class SourcePrice
    {
        public int Id { get; set; }
        public int SupplySourceId { get; set; }


        [Required]
        public float UnitPrice { get; set; }
        public float Quantity { get; set; } = 1;
        public string PriceString { get => $"{Quantity}+ {string.Format("{0:0.00000}",UnitPrice)}"; }

    }
}