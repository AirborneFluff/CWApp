using System.ComponentModel.DataAnnotations;

namespace API.DTOs.PartsListDTOs
{
    public class NewPartsListEntryDto
    {
        [Required]
        public string PartCode { get; set; }
        [Required]
        public float Quantity { get; set; }
    }
}