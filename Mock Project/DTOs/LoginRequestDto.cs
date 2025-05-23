using System.ComponentModel.DataAnnotations;

namespace Mock_Project.DTOs
{
    public class LoginRequestDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }

}
