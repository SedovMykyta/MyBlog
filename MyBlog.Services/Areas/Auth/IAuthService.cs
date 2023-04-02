using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Auth;

public interface IAuthService
{
    public Task RegisterAsync(UserInputDto userInput, bool isSubscribeToEmail);
    
    public Task<string> LoginAsync(UserLoginDto userLogin);

}