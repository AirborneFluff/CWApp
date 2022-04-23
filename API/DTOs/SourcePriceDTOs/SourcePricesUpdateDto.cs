using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace API.DTOs.SourcePriceDTOs
{
    public class SourcePricesUpdateDto
    {
        [Required]
        public Price[] Prices { get; set; }
    }

    public class Price {
        public float UnitPrice { get; set; }
        public float Quantity { get; set; } = 1;
    }
}