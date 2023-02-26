using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Areas.Users.Dto;

namespace MyBlog.Service.Areas.Users;

public interface IUserService
{
    public Task<List<UserDto>> GetListAsync();

    public Task<UserDto> GetByIdAsync(int id);
    
    public Task<UserDto> GetByEmailAsync(string email);

    public Task<User> GetByLoginAsync(UserDtoLogin userLogin);

    public Task<UserDto> CreateAsync(UserDtoInput userInput);
    
    public Task<UserDto> UpdateByIdAsync(int id, UserDtoInput userInput);
    
    public Task DeleteByIdAsync(int id);
}