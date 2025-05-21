namespace Mock_Project.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public string Base64Image { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}






