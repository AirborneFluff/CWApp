using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string NormalizedName { get => Name.ToUpper(); set => Name.ToUpper(); }
        public string Description { get; set; }
    [JsonPropertyName("boms")]
        public ICollection<BOM> BOMs { get; set; }
    }
}