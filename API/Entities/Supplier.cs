using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Website { get; set; }
        public string NormalizedName { get => Name.ToUpper(); set => Name.ToUpper(); }
    }
}