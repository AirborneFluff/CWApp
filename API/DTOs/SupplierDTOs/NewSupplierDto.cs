using System.ComponentModel.DataAnnotations;

namespace API.DTOs.SupplierDTOs
{
    public class NewSupplierDto
    {
        [Required]
        public string Name { get; set; }
        public string Website { get; set; }
    }
}