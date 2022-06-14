namespace API.DTOs.OutboundOrderDTOs
{
    public class NewOutboundOrderItemDTO
    {
        public string Partcode { get; set; }

        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}