using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Users;

public class UserService : IUserService
{
    private readonly MyBlogContext _context;
    private readonly IMapper _mapper;
    
    public UserService(MyBlogContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> GetListAsync()
    {
        var users = await _context.Users
            .Select(u => _mapper.Map<UserDto>(u))
            .ToListAsync();
        
        return users;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email)
                   ?? throw new NotFoundException($"User with Email: {email} is not fount");
        
        return user;
    }

    public async Task<int> CreateAsync(UserDtoInput userInput)
    {
        if (_context.Users.Any(u => u.Email == userInput.Email || u.Phone == userInput.Phone))
        {
            throw new ExistsException($"User with Email: {userInput.Email} or phone: {userInput.Phone} exists");
        }
        
        var user = _mapper.Map<User>(userInput);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user.Id;
    } 

    public async Task<int> UpdateByIdAsync(int id, UserDtoInput userInput)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

        if (CheckForFreeAsync(id, userInput).Result == false)
        {
            throw new ExistsException($"User with Email: {userInput.Email} or phone: {userInput.Phone} exists");
        }
        
        _mapper.Map(userInput, user);
        
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
    
    private async Task<bool> CheckForFreeAsync(int id, UserDtoInput userInput)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id)!;

        var repeatUserEmail = user.Email == userInput.Email;
        var repeatUserPhone = user.Phone == userInput.Phone;

        if (repeatUserEmail && repeatUserPhone)
        {
            return true;
        }
        
        if (repeatUserEmail == false && repeatUserPhone == false)
        {
            if (_context.Users.Any(u => u.Email == userInput.Email || u.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (repeatUserEmail)
        {
            if (_context.Users.Any(u => u.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (repeatUserPhone)
        {
            if (_context.Users.Any(u => u.Email == userInput.Email))
            {
                return false;
            }
        }

        return true;
    }
}