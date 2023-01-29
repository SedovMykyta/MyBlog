using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Users;

public class UserService : IUserService
{
    private readonly MyBlogContext _db;

    public UserService(MyBlogContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Id == id)
               ?? throw new NotFoundException($"User with Id: {id} is not found");
    }

    public async Task CreateAsync(User userInput)
    {
        await _db.Users.AddAsync(userInput);
    }

    public async Task UpdateByIdAsync(int id, User userInput)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");
        
        user.FirstName = userInput.FirstName;
        user.LastName = userInput.LastName;
        user.Password = userInput.Password;
        user.Email = userInput.Email;
        user.Phone = userInput.Phone;

        _db.Users.Update(user);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

        _db.Users.Remove(user);
    }
}