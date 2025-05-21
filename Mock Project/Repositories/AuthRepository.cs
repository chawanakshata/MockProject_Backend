using Microsoft.EntityFrameworkCore;
using Mock_Project.Data;
using Mock_Project.Models;
using Mock_Project.Repositories;


public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<User> LoginAsync(string username, string password)
    {
        var loginRequest = await _context.LoginRequestTable
            .Include(lr => lr.User)
            .FirstOrDefaultAsync(lr => lr.Username == username && lr.Password == password);

        if (loginRequest == null || loginRequest.User == null)
        {
            Console.WriteLine($"Login failed for username: {username}");
            return null;
        }

        return loginRequest.User;
    }

    public async Task<User> RegisterAsync(string username, string password)
    {
        // Check if username already exists in LoginRequest
        var exists = await _context.LoginRequestTable.AnyAsync(lr => lr.Username == username);
        if (exists)
            throw new Exception("Username already exists.");

        // Create the User first
        var newUser = new User
        {
            Name = username,
            Role = "Admin"
            // Do NOT set Password here if you only want it in LoginRequest
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // Now create LoginRequest with the same Id as User
        var loginRequest = new LoginRequest
        {
            Id = newUser.Id, // Shared PK/FK
            Username = username,
            Password = password
        };

        _context.LoginRequestTable.Add(loginRequest);
        await _context.SaveChangesAsync();

        return newUser;
    }

}
