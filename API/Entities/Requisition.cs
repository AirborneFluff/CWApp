using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Requisition
    {
        public int Id { get; set; }
        [Required]
        public int PartId { get; set; }
        [JsonIgnore]
        public Part Part { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int? OutboundOrderId { get; set; }
        public OutboundOrder OutboundOrder { get; set; }


        [Required]
        public float Quantity { get; set; }
        public bool ForBuffer { get; set; }
        public bool Urgent { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}