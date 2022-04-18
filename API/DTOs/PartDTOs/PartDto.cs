using System.ComponentModel.DataAnnotations;

namespace API.DTOs.PartDTOs
{
    public class PartDto
    {
        public string PartCode { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Buffer { get; set; }
    }
}