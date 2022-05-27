using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public string PartCode { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public float BufferValue { get; set; }
        public string BufferUnit { get; set; }
        public ICollection<SupplySource> SupplySources { get; set; }

        public string Buffer { get { return $"{BufferValue} {BufferUnit}"; } }


        public ICollection<OutboundOrderItem> Orders { get; set; }
        public ICollection<Requisition> Requisitions { get; set; }
    }
}