using System.ComponentModel.DataAnnotations;

namespace API.DTOs.SupplierDTOs
{
    public class SupplierNameDto
    {
        [Required]
        public string Name { get; set; }
    }
}