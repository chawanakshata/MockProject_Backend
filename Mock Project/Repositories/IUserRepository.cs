////using Mock_Project.Models;

////namespace Mock_Project.Repositories
////{
////    public interface IUserRepository
////    {
////        Task<User> GetSingleUserAsync();
////        Task AddAsync(User user);
////        Task UpdateAsync(User user);
////        Task DeleteAsync(int id);

////    }
////}

//using Mock_Project.Models;

//namespace Mock_Project.Repositories
//{
//    public interface IUserRepository
//    {
//        Task<User> GetSingleUserAsync();
//        Task AddAsync(User user);
//        Task UpdateAsync(User user);
//        Task DeleteAsync(int id);
//        Task<User> GetByIdAsync(int id);
//        Task AddFactAsync(UserFact fact); 
//        Task<UserFact> GetFactByIdAsync(int factId);
//        Task UpdateFactAsync(UserFact fact);
//        Task DeleteFactAsync(int factId); 
//    }
//}

using System.Collections.Generic;
using System.Threading.Tasks;
using Mock_Project.Models;

namespace Mock_Project.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetSingleUserAsync();
        Task<User> GetByIdAsync(int id); // Add this line
        Task<IEnumerable<User>> GetAllUsersAsync(); // Add this line
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task AddFactAsync(UserFact fact);
        Task<UserFact> GetFactByIdAsync(int factId);
        Task UpdateFactAsync(UserFact fact);
        Task DeleteFactAsync(int factId);
    }
}
