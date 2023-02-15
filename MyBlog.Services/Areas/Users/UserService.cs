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
            .Select(user => _mapper.Map<UserDto>(user))
            .ToListAsync();
        
        return users;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);

        return user;
    }

    public async Task<UserDto> CreateAsync(UserDtoInput userInput)
    {
        if (_context.Users.Any(u => u.Email == userInput.Email || u.Phone == userInput.Phone))
        {
            throw new BadRequestException($"User with this email or phone exists");
        }
        
        var user = _mapper.Map<User>(userInput);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    } 

    public async Task<UserDto> UpdateByIdAsync(int id, UserDtoInput userInput)
    {
        var user = await GetUserByIdAsync(id);

        var freeEmailAndPhone = await CheckEmailAndPhoneForFreeAsync(id, userInput);
        if (freeEmailAndPhone == false)
        {
            throw new BadRequestException($"User with this email or phone exists");
        }
        
        _mapper.Map(userInput, user);
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await GetUserByIdAsync(id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    private async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id) 
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

        return user;
    }

    private async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email)
                   ?? throw new NotFoundException($"User with Email: {email} is not fount");

        return user;
    }
    
    private async Task<bool> CheckEmailAndPhoneForFreeAsync(int id, UserDtoInput userInput)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        bool repeatUserEmail = user.Email == userInput.Email;
        bool repeatUserPhone = user.Phone == userInput.Phone;

        if (repeatUserEmail && repeatUserPhone)
        {
            return true;
        }
        
        if (repeatUserEmail == false && repeatUserPhone == false)
        {
            if (_context.Users.Any(user => user.Email == userInput.Email || user.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (repeatUserEmail)
        {
            if (_context.Users.Any(user => user.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (repeatUserPhone)
        {
            if (_context.Users.Any(user => user.Email == userInput.Email))
            {
                return false;
            }
        }

        return true;
    }
}