using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class NewUserDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]

        public string Email { get; set; }
    }
}
