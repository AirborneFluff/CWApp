using System.ComponentModel.DataAnnotations;

namespace API.DTOs.PartDTOs
{
    public class NewPartDto
    {
        [Required]
        public string PartCode { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public float BufferValue { get; set; }
        public string BufferUnit { get; set; }
    }
}