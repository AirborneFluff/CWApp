using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Requisition
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }


        [Required]
        public float Quantity { get; set; }
        public string QuantityUnits { get; set; }
        public float StockRemaining { get; set; }
        public bool ForBuffer { get; set; }
        public bool Urgent { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}