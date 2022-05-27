using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTOs.UserDTOs
{
    public class NewUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}