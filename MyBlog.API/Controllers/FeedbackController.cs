using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Rating;
using MyBlog.Service.Helpers.ClaimParser;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Controllers;

[Route("api/feedback")]
[ApiController]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class FeedbackController : ControllerBase
{
    private readonly IRatingService _ratingService;
    private readonly Task<JwtInfoDto> _currentUserJwtInfo;

    public FeedbackController(IRatingService ratingService, IClaimsParser parser)
    {
        _ratingService = ratingService;
        _currentUserJwtInfo = Task.Run(() => parser.ToJwtInfo(HttpContext.User.Identity as ClaimsIdentity));
    }
    
    /// <summary>
    /// Like Article by id
    /// </summary>
    /// <param name="articleId">Article id</param>
    /// <returns>Returns OK</returns>
    /// <response code="200">Success</response>
    /// <response code="404">ArticleNotFound</response>
    [HttpPost ("like")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LikeByIdAsync([FromForm] int articleId)
    {
        await _ratingService.LikeByIdAsync(articleId, _currentUserJwtInfo.Result.Id);
        
        return Ok();
    }
    
    /// <summary>
    /// Dislike Article by id
    /// </summary>
    /// <param name="articleId">Article id</param>
    /// <returns>Returns OK</returns>
    /// <response code="200">Success</response>
    /// <response code="404">ArticleNotFound</response>
    [HttpPost ("dislike")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DislikeByIdAsync([FromForm] int articleId)
    {
        await _ratingService.DislikeByIdAsync(articleId, _currentUserJwtInfo.Result.Id);
        
        return Ok();
    }
}