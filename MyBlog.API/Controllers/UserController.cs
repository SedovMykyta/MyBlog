using Microsoft.AspNetCore.Mvc;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Users;

namespace MyBlog.Controllers;

[ApiController]
[Route("[controller]")]
public class UserTestController : ControllerBase
{
    private readonly ILogger<UserTestController> _logger;
    private readonly IUserService _userService;
    
    public UserTestController(ILogger<UserTestController> logger, IUserService userService)
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [HttpGet( "{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var user = await _userService.GetByIdAsync(id);
    
        return Ok(user);
    }
    
    /// <summary>
    /// Creates a User.
    /// </summary>
    /// <param name="user">User object</param>
    /// <returns>Id created user</returns>
    /// <response code="200">Success</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] User user)
    {
        var idUser = await _userService.CreateAsync(user);
        
        return Ok(idUser);
    }
    
    /// <summary>
    /// Update User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="user">User object</param>
    /// <returns>Id updated user</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateByIdAsync(int id, [FromBody] User user)
    {
        var idUser = await _userService.UpdateByIdAsync(id, user);
        
        return Ok(idUser);
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
    public async Task<IActionResult> DeleteByIdAsync(int id)
    {
        await _userService.DeleteByIdAsync(id);

        return Ok();
    }
}