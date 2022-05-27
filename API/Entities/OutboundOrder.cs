namespace API.Entities
{
    public class OutboundOrder
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }

        public int OrderNumber { get; set; }
        public string SupplierReference { get; set; }
        public DateTime TaxDate { get; set; }

        public ICollection<OutboundOrderItem> Items { get; set; }
    }
}