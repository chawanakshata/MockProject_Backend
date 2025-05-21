namespace Mock_Project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureBase64 { get; set; }
        public ICollection<UserImage> Gallery { get; set; }
        public ICollection<UserFact> Facts { get; set; }
        public LoginRequest LoginRequest { get; set; }
        public string Role { get; set; }
    }
}





