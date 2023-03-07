using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Mailing;
using MyBlog.Service.Helpers.ClaimParser;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Controllers;

[Route("api/article")]
[ApiController]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class MailingController : ControllerBase
{
    private readonly IMailingService _mailingService;
    private readonly Task<JwtInfoDto> _currentUserJwtInfo;

    public MailingController(IMailingService mailingService, IClaimsParser parse)
    {
        _mailingService = mailingService;
        _currentUserJwtInfo = Task.Run(() => parse.ToJwtInfo(HttpContext.User.Identity as ClaimsIdentity));

    }
    
    /// <summary>
    /// Send email to user
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="recipientEmail">Email recipient</param>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserWithThisEmailNotFound</response>
    [HttpPost("toUser")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendEmailToUserAsync([FromForm] string message, [FromForm] string recipientEmail)
    {
        await _mailingService.SendEmailToUserAsync(message, recipientEmail);

        return Ok();
    }

    /// <summary>
    /// Send email to subscribed user
    /// </summary>
    /// <param name="message">Message</param>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="404">NotSubscribedUsers</response>
    [HttpPost ("toSubscribed")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendEmailToSubscribedUsersAsync([FromForm] string message)
    {
        await _mailingService.SendEmailToSubscribedUsersAsync(message);

        return Ok();
    }

    /// <summary>
    /// Change subscribe on mailing
    /// </summary>
    /// <param name="isSubscribeToEmail"></param>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="400">UserNotFound</response>
    [HttpPost ("changeSubscribe")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task ChangeSubscribeOnMailingAsync([FromForm] bool isSubscribeToEmail)
    {
        await _mailingService.ChangeSubscribeOnMailingAsync(isSubscribeToEmail, _currentUserJwtInfo.Result.Id);
    }
}