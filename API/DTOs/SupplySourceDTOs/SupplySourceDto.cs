using System.ComponentModel.DataAnnotations;

namespace API.DTOs.SupplySourceDTOs
{
    public class SupplySourceDto
    {
        [Required]
        public string SupplierName { get; set; }
        public string SupplierSKU { get; set; }
        public string ManufacturerSKU { get; set; }
        public float PackSize { get; set; } = 1;
        public float MinimumOrderQuantity { get; set; } = 1;
        public string Notes { get; set; }
        public bool RoHS { get; set; }
    }
}