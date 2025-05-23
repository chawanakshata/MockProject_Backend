namespace Mock_Project.Repositories
{
    public interface IFileConverterService
    {
        Task<string> ConvertToBase64Async(IFormFile file);
    }
}
