using System.ComponentModel.DataAnnotations;

namespace API.DTOs.BOMDTOs
{
    public class UpdateBOMEntryDto
    {
        [Required]
        public float Quantity { get; set; }
        public string ComponentLocation { get; set; }
    }
}