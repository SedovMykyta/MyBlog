using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    
    public AuthService(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    public async Task RegisterAsync(UserDtoInput userInput)
    {
         await _userService.CreateAsync(userInput);
    }

    public async Task<string> LoginAsync(UserDtoLogin userLogin)
    {
        var user = AuthenticateAsync(userLogin);

        var token = GenerateJwtTokenAsync(await user);
        
        return await token;
    }

    private async Task<User> AuthenticateAsync(UserDtoLogin userLogin)
    {
        var user = await _userService.GetByEmailAsync(userLogin.Email);

        if (userLogin.Password != user.Password)
        {
            throw new BadRequestException("You entering wrong password");
        }
        
        return user;
    }

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}