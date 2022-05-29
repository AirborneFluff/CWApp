using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string Initials { get => $"{FirstName.First()}{LastName.First()}"; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}