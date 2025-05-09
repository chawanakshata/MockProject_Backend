namespace Mock_Project.Models
{
    public class UserFact
    {
        public int Id { get; set; }
        public string Fact { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
