namespace API.DTOs.RequisitionDTOs
{
    public class RequisitionDetailsDto
    {
        public int Id { get; set; }
        public Requisition_PartDto Part { get; set; }
        public Requisition_UserDto User { get; set; }
        public Requisition_OutboundOrderDto OutboundOrder { get; set; }


        public float Quantity { get; set; }
        public bool ForBuffer { get; set; }
        public bool Urgent { get; set; }
        public DateTime Date { get; set; }
    }

    public class Requisition_PartDto
    {
        public string PartCode { get; set; }
        public string Description { get; set; }
        public string StockUnits { get; set; }
    }

    public class Requisition_OutboundOrderDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int OrderNumber { get; set; }
    }
    public class Requisition_UserDto
    {
        public string Initials { get; set; }
    }
}