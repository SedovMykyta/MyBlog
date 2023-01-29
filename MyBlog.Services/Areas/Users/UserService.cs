using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Users;

public class UserService : IUserService
{
    private readonly MyBlogContext _context;

    public UserService(MyBlogContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetListAsync()
    {
        var userList = await _context.Users.ToListAsync();
        
        return userList;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

        return user;
    }

    public async Task<int> CreateAsync(User userInput)
    {
        await _context.Users.AddAsync(userInput);
        await _context.SaveChangesAsync();

        return userInput.Id; 
    } 

    public async Task<int> UpdateByIdAsync(int id, User userInput)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");
        
        user.FirstName = userInput.FirstName;
        user.LastName = userInput.LastName;
        user.Password = userInput.Password;
        user.Email = userInput.Email;
        user.Phone = userInput.Phone;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}