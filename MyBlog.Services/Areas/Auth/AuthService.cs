﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;
using MyBlog.Service.Areas.Users.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.PasswordManagers;

namespace MyBlog.Service.Areas.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    private readonly IPasswordManager _passwordManager;
    private readonly MyBlogContext _context;
    
    public AuthService(IUserService userService, IConfiguration config, IPasswordManager passwordManager, MyBlogContext context)
    {
        _userService = userService;
        _config = config;
        _passwordManager = passwordManager;
        _context = context;
    }

    public async Task RegisterAsync(UserInputDto userInput, bool isSubscribeToEmail)
    {
         var userDto = await _userService.CreateAsync(userInput);

         var userSubscription = new UserSubscription
         { 
             UserId = userDto.Id,
             IsSubscribedToEmail = isSubscribeToEmail
         };

         await _context.UserSubscriptions.AddAsync(userSubscription);
         await _context.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(UserLoginDto userLogin)
    {
        var user = await AuthenticateAsync(userLogin);

        var token = GenerateJwtToken(user);
        
        return token;
    }

    private async Task<User> AuthenticateAsync(UserLoginDto userLogin)
    {
        var user = await _userService.GetByLoginAsync(userLogin);
        if (userLogin.Password != _passwordManager.Decrypt(user.Password))
        {
            throw new BadRequestException("You entering wrong password");
        }
        
        return user;
    }

    private string GenerateJwtToken(User user)
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

        JwtSecurityToken token = new (
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}