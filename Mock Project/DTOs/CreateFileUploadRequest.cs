namespace Mock_Project.DTOs
{
    public class CreateFileUploadRequest
    {
        public string TeamMemberName { get; set; }
        public IFormFile? File { get; set; }
    }
}
