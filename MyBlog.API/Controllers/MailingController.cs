using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Mailing;
using MyBlog.Service.Helpers.ClaimParser;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Controllers;

[Route("api/mailing")]
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
    public async Task<IActionResult> SendEmailToUserAsync([FromForm] string recipientEmail, [FromForm] string message)
    {
        await _mailingService.SendEmailToUserAsync(recipientEmail, message);

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
    /// Subscribe on mailing
    /// </summary>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="400">UserNotFound</response>
    [HttpPost ("changeSubscribe")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task SubscribeOnMailingAsync()
    {
        await _mailingService.SubscribeOnMailingAsync(_currentUserJwtInfo.Result.Id);
    }
    
    /// <summary>
    /// Unsubscribe on mailing
    /// </summary>
    /// <returns>Returns Ok</returns>
    /// <response code="200">Success</response>
    /// <response code="400">UserNotFound</response>
    [HttpPost ("changeSubscribe")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task UnsubscribeOnMailingAsync()
    {
        await _mailingService.UnsubscribeOnMailingAsync(_currentUserJwtInfo.Result.Id);
    }
}