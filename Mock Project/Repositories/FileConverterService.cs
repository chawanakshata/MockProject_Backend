using Mock_Project.Exceptions;

namespace Mock_Project.Repositories
{
    public class FileConverterService : IFileConverterService
    {
        public async Task<string> ConvertToBase64Async(IFormFile file)
        {
            if (file == null || file.Length == 0) // Check if the file is null or empty
                throw new BadRequestException("File is null or empty."); 

            using var memoryStream = new MemoryStream(); // Create a memory stream to hold the file data
            await file.CopyToAsync(memoryStream); // Copy the file data to the memory stream
            return Convert.ToBase64String(memoryStream.ToArray()); // Convert the memory stream to a byte array and then to a Base64 string
        }
    }
}
