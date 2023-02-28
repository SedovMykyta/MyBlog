using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Areas.Users.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ExtensionMethods;
using MyBlog.Service.Helpers.PasswordManagers;

namespace MyBlog.Service.Areas.Users;

public class UserService : IUserService
{
    private readonly MyBlogContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordManager _passwordManager;
    
    public UserService(MyBlogContext context, IMapper mapper, IPasswordManager passwordManager)
    {
        _context = context;
        _mapper = mapper;
        _passwordManager = passwordManager;
    }

    public async Task<List<UserDto>> GetListAsync()
    {
        var users = await _context.Users
            .Select(user => _mapper.Map<UserDto>(user))
            .ThrowIfEmpty()
            .ToListAsync();
        
        return users;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<User> GetByLoginAsync(UserDtoLogin userLogin)
    {
        var user = await GetUserByEmailAsync(userLogin.Email);

        return user;
    }
    
    public async Task<UserDto> CreateAsync(UserDtoInput userInput)
    {
        await ThrowIfEmailOrPhoneExistAsync(userInput);

        userInput.Password = _passwordManager.Encrypt(userInput.Password);
        
        var user = _mapper.Map<User>(userInput);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    } 
    
    public async Task<UserDto> UpdateByIdAsync(int id, UserDtoInput userInput)
    {
        var user = await GetUserByIdAsync(id);

        await ThrowIfEmailOrPhoneExistAsync(userInput, id);

        userInput.Password = _passwordManager.Encrypt(userInput.Password);
        
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

    private async Task ThrowIfEmailOrPhoneExistAsync(UserDtoInput userInput, int id = -1)
    {
        var isEmailBusy = await _context.Users.AnyAsync(user => user.Email == userInput.Email && user.Id != id);
        var isPhoneBusy = await _context.Users.AnyAsync(user => user.Phone == userInput.Phone && user.Id != id);

        if (isEmailBusy && isPhoneBusy)
        {
            throw new BadRequestException($"User with this email and phone exist");
        }
        if (isEmailBusy)
        {
            throw new BadRequestException($"User with this email exist");
        }
        if (isPhoneBusy)
        {
            throw new BadRequestException($"User with this phone exist");
        }
    }
}