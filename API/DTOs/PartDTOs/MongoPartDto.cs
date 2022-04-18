namespace API.DTOs.PartDTOs
{
    public class MongoPartDto
    {
        public string PartCode { get; set; }
        public string Description { get; set; }
        public float BufferValue { get; set; }
        public string BufferSuffix { get; set; }

        List<MongoSource> SupplierSources { get; set; }
    }

    class MongoSource
    {
        public string SupplierName { get; set; }
        public string SupplierSKU { get; set; }
        public string ManufacturerSKU { get; set; }
        public float PackSize { get; set; }
        public float MinimumOrder { get; set; }
        public string Notes { get; set; }
        public bool RoHS { get; set; }
        
        List<MongoPrice> Pricebreaks { get; set; }
    }

    class MongoPrice 
    {
        public float UnitPrice { get; set; }
        public float Quantity { get; set; }
    }
}