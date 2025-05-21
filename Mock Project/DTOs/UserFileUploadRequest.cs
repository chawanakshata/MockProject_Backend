namespace Mock_Project.DTOs
{
    public class UserFileUploadRequest
    {
        public string UserName { get; set; }
        public string Bio { get; set; }
        public IFormFile? File { get; set; }

    }
}


