using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Areas.Users.Dto;

namespace MyBlog.Service.Areas.Auth;

public interface IAuthService
{
    public Task RegisterAsync(UserDtoInput userInput, bool isSubscribeToEmail);
    
    public Task<string> LoginAsync(UserDtoLogin userLogin);

}