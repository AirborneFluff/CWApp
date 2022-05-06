using System.ComponentModel.DataAnnotations;

namespace API.DTOs.BOMDTOs
{
    public class NewBOMEntryDto
    {
        [Required]
        public string PartCode { get; set; }
        [Required]
        public float Quantity { get; set; }
    }
}