using System.ComponentModel.DataAnnotations;

namespace API.DTOs.OutboundOrderDTOs
{
    public class NewOutboundOrderDTO
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public int OrderNumber { get; set; }
        public string SupplierReference { get; set; }
        public DateTime TaxDate { get; set; }
    }
}