using Mock_Project.Models;

namespace Mock_Project.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<UserImageDto> Gallery { get; set; }
        public ICollection<UserFactDto> Facts { get; set; }
    }
}
