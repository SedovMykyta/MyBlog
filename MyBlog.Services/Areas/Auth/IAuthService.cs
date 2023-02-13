using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Auth;

public interface IAuthService
{
    public Task RegisterAsync(UserDtoInput userInput);
    
    public Task<string> LoginAsync(UserDtoLogin userLogin);

}