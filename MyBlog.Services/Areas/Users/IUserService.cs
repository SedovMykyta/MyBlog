using MyBlog.Infrastructure.Entities;

namespace MyBlog.Service.Areas.Users;

public interface IUserService
{
    Task<List<User>> GetAllAsync();

    Task<User> GetByIdAsync(int id);

    Task CreateAsync(User userInput);
    
    Task UpdateByIdAsync(int id, User userInput);
    
    Task DeleteByIdAsync(int id);
}