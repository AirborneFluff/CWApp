using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ProductsDTOs
{
    public class NewProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}