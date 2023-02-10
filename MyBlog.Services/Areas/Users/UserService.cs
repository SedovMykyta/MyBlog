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

    public async Task<int> CreateAsync(UserDtoInput userInput)
    {
        var user = _mapper.Map<User>(userInput);
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user.Id;
    } 

    public async Task<int> UpdateByIdAsync(int id, UserDtoInput userInput)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
                   ?? throw new NotFoundException($"User with Id: {id} is not found");

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
}