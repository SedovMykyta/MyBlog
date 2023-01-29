using MyBlog.Infrastructure.Entities;

namespace MyBlog.Service.Areas.Users;

public interface IUserService
{
    public Task<List<User>> GetListAsync();

    public Task<User> GetByIdAsync(int id);

    public Task<int> CreateAsync(User userInput);
    
    public Task<int> UpdateByIdAsync(int id, User userInput);
    
    public Task DeleteByIdAsync(int id);
}