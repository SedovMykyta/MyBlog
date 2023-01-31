using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users;

namespace MyBlog.Controllers;

[ApiController]
[Route("api/users")]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    
    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    /// <summary>
    /// Get list of Users
    /// </summary>
    /// <returns>Returns User list</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync()
    {
        var users = await _userService.GetListAsync();
        
        return Ok(users);
    }

    /// <summary>
    /// Get User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>Returns User</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var user = await _userService.GetByIdAsync(id);
    
        return Ok(user);
    }

    /// <summary>
    /// Create User
    /// </summary>
    /// <param name="userInput">User object</param>
    /// <returns>Created User object</returns>
    /// <response code="200">Success</response>
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] User userInput)
    {
        var userId = await _userService.CreateAsync(userInput);
        var user = await _userService.GetByIdAsync(userId);
        
        return Ok(user);
    }
    
    /// <summary>
    /// Update User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="userInput">User object</param>
    /// <returns>Updated User object</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpPut ("{id:int}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] User userInput)
    {
        var userId = await _userService.UpdateByIdAsync(id, userInput);
        var user = await _userService.GetByIdAsync(userId);
        
        return Ok(user);
    }


    /// <summary>
    /// Delete User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpDelete ("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _userService.DeleteByIdAsync(id);

        return Ok();
    }
}