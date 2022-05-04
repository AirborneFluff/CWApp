using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class PartsList
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<PartsListEntry> Parts { get; set; }
    }
}