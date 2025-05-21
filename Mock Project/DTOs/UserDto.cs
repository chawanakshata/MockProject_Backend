using Mock_Project.DTOs;
namespace Mock_Project.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string ProfilePictureBase64 { get; set; }
        public List<UserImageDto> Gallery { get; set; }
        public List<UserFactDto> Facts { get; set; }

    }
}

