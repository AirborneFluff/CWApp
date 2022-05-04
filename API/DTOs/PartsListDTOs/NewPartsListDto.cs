using System.ComponentModel.DataAnnotations;

namespace API.DTOs.PartsListDTOs
{
    public class NewPartsListDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}