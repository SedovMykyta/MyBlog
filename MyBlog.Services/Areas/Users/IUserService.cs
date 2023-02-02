using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Users;

public interface IUserService
{
    public Task<List<UserDtoGet>> GetListAsync();

    public Task<UserDtoGet> GetByIdAsync(int id);

    public Task<int> CreateAsync(UserDtoInput userInput);
    
    public Task<int> UpdateByIdAsync(int id, UserDtoInput userInput);
    
    public Task DeleteByIdAsync(int id);
}