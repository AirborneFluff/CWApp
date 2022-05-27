using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string Initials { get => $"{FirstName.First()}{LastName.First()}"; }
    }
}