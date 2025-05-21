namespace Mock_Project.DTOs
{
    public class UserUpdateRequest
    {
        public string UserName { get; set; }
        public string Bio { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public List<IFormFile> GalleryImages { get; set; }
    }
}
