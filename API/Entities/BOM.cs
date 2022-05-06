using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class BOM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<BOMEntry> Parts { get; set; }
    }
}