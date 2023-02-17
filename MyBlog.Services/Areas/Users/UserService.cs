using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.EncryptDecrypt;

namespace MyBlog.Service.Areas.Users;

public class UserService : IUserService
{
    private readonly MyBlogContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordManager _password;
    
    public UserService(MyBlogContext context, IMapper mapper, IPasswordManager password)
    {
        _context = context;
        _mapper = mapper;
        _password = password;
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
        if (_context.Users.Any(user => user.Email == userInput.Email || user.Phone == userInput.Phone))
        {
            throw new BadRequestException($"User with this email or phone exists");
        }

        userInput.Password = _password.Encrypt(userInput.Password);
        
        var user = _mapper.Map<User>(userInput);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    } 

    public async Task<UserDto> UpdateByIdAsync(int id, UserDtoInput userInput)
    {
        var user = await GetUserByIdAsync(id);

        var isEmailAndPhoneAreFree = await CheckEmailAndPhoneAreFreeAsync(id, userInput);
        if (! isEmailAndPhoneAreFree)
        {
            throw new BadRequestException($"User with this email or phone exists");
        }
        
        userInput.Password = _password.Encrypt(userInput.Password);
        
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
                   ?? throw new NotFoundException($"User with Email: {email} is not found");

        return user;
    }
    
    private async Task<bool> CheckEmailAndPhoneAreFreeAsync(int id, UserDtoInput userInput)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        bool isUserEmailRepeat = user.Email == userInput.Email;
        bool isUserPhoneRepeat = user.Phone == userInput.Phone;

        if (isUserEmailRepeat && isUserPhoneRepeat)
        {
            return true;
        }
        
        if (! isUserEmailRepeat && ! isUserPhoneRepeat)
        {
            if (_context.Users.Any(user => user.Email == userInput.Email || user.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (isUserEmailRepeat)
        {
            if (_context.Users.Any(user => user.Phone == userInput.Phone))
            {
                return false;
            }
        }
        
        if (isUserPhoneRepeat)
        {
            if (_context.Users.Any(user => user.Email == userInput.Email))
            {
                return false;
            }
        }

        return true;
    }
}