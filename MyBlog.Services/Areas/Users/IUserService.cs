using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Users;

public interface IUserService
{
    public Task<List<UserDto>> GetListAsync();

    public Task<UserDto> GetByIdAsync(int id);
    
    public Task<UserDto> GetByEmailAsync(string email);

    protected internal Task<User> GetByLoginAsync(UserLoginDto userLogin);

    public Task<UserDto> CreateAsync(UserInputDto userInput);
    
    public Task<UserDto> UpdateByIdAsync(int id, UserInputDto userInput);
    
    public Task DeleteByIdAsync(int id);
}