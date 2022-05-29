using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RequisitionDTOs
{
    public class NewRequisitionDto
    {
        [Required]
        public int PartId { get; set; }
        public int UserId { get; set; }

        [Required]
        public float Quantity { get; set; }
        public float StockRemaining { get; set; }
        public bool ForBuffer { get; set; }
        public bool Urgent { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}