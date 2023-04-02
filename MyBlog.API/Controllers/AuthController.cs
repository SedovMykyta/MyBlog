using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Auth;
using MyBlog.Service.Areas.Mailing;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Controllers;

[ApiController]
[Route("api/auth")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMailingService _mailingService;
    
    public AuthController(IAuthService authService, IMailingService mailingService)
    {
        _authService = authService;
        _mailingService = mailingService;
    }

    /// <summary>
    /// Register new account
    /// </summary>
    /// <param name="userInput">UserInputDtoDto object</param>
    /// <param name="isSubscribeToEmail">bool answer</param>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="400">UserWithThisParameterExists</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAsync([FromForm] UserInputDto userInput, [FromQuery] bool isSubscribeToEmail)
    {
        await _authService.RegisterAsync(userInput, isSubscribeToEmail);

        await _mailingService.SendEmailToUserAsync(userInput.Email, "You successfully registered on site 'My Blog'.");

        return Ok();
    }
    
    
    /// <summary>
    /// Login in account
    /// </summary>
    /// <param name="userLogin">UserLoginDtoDto object</param>
    /// <returns>Returns JWT token</returns>
    /// <response code="200">Success</response>
    /// <response code="400">BadRequest</response>
    /// <response code="404">UserNotFound</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginAsync([FromForm] UserLoginDto userLogin)
    {
        var token = await _authService.LoginAsync(userLogin);

        return Ok(token);
    }

}