using System.ComponentModel.DataAnnotations;

namespace API.DTOs.BOMDTOs
{
    public class NewBOMDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}