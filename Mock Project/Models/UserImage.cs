namespace Mock_Project.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
