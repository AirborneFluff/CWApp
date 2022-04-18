using System.ComponentModel.DataAnnotations;

namespace API.DTOs.SourcePriceDTOs
{
    public class SourcePriceDto
    {
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string SupplierSKU { get; set; }

        public float UnitPrice { get; set; }
        public float Quantity { get; set; } = 1;
    }
}