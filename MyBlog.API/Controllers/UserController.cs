using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Areas.Users.AutoMapper.Dto;

namespace MyBlog.Controllers;

[ApiController]
[Route("api/users")]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Get list of Users
    /// </summary>
    /// <returns>Returns UserDto list</returns>
    /// <response code="200">Success</response>
    /// <response code="404">CollectionIsEmpty</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListAsync()
    {
        var users = await _userService.GetListAsync();
        
        return Ok(users);
    }


    /// <summary>
    /// Get User by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <returns>Returns UserDto</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpGet("{email}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var userDto = await _userService.GetByEmailAsync(email);

        return Ok(userDto);
    }

    /// <summary>
    /// Get User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>Returns UserDto</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var userDto = await _userService.GetByIdAsync(id);

        return Ok(userDto);
    }

    /// <summary>
    /// Create User
    /// </summary>
    /// <param name="userInput">UserDtoInput object</param>
    /// <returns>Created UserDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="400">EmailOrPhoneExists</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] UserDtoInput userInput)
    {
        var userDto = await _userService.CreateAsync(userInput);

        return Ok(userDto);
    }
    
    /// <summary>
    /// Update User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="userInput">UserDtoInput object</param>
    /// <returns>Updated UserDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="400">EmailOrPhoneExists</response>
    /// <response code="404">UserNotFound</response>
    [HttpPut ("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromBody] UserDtoInput userInput)
    {
        var userDto = await _userService.UpdateByIdAsync(id, userInput);

        return Ok(userDto);
    }


    /// <summary>
    /// Delete User by id
    /// </summary>
    /// <param name="id">User id</param>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    [HttpDelete ("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _userService.DeleteByIdAsync(id);

        return Ok();
    }
}