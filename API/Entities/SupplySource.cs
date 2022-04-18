using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class SupplySource
    {
        public int Id { get; set; }
        public int PartId { get; set; }

        [Required]
        public Supplier Supplier { get; set; }
        [Required]
        public string SupplierSKU { get; set; }
        public string ManufacturerSKU { get; set; }
        public float PackSize { get; set; } = 1;
        public float MinimumOrderQuantity { get; set; } = 1;
        public string Notes { get; set; }
        public bool RoHS { get; set; }

        public ICollection<SourcePrice> Prices { get; set; }
    }
}