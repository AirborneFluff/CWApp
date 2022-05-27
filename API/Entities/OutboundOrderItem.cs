namespace API.Entities
{
    public class OutboundOrderItem
    {
        public int Id { get; set; }
        public int OutboundOrderId { get; set; }

        public Part Part { get; set; }
        public int PartId { get; set; }

        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}