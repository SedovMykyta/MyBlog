using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Areas.Comments;
using MyBlog.Service.Areas.Comments.AutoMapper.Dto;
using MyBlog.Service.Areas.Rating;
using MyBlog.Service.Helpers.ClaimParser;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Controllers;

[ApiController]
[Route("api/feedback")]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class FeedbackController : ControllerBase
{
    private readonly IRatingService _ratingService;
    private readonly ICommentService _commentService;
    private readonly Task<JwtInfoDto> _currentUserJwtInfo;

    public FeedbackController(IRatingService ratingService, ICommentService commentService, IClaimsParser parser)
    {
        _ratingService = ratingService;
        _commentService = commentService;
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

    /// <summary>
    /// Get list of Comments
    /// </summary>
    /// <returns>Returns CommentDto list</returns>
    /// <response code="200">Success</response>
    /// <response code="404">CollectionIsEmpty</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListAsync()
    {
        var comments = await _commentService.GetListAsync();

        return Ok(comments);
    }

    /// <summary>
    /// Get list of Comments by article id
    /// </summary>
    /// <returns>Returns CommentDto list</returns>
    /// <response code="200">Success</response>
    /// <response code="404">CollectionIsEmpty</response>
    [HttpGet("articleId/{articleId:int}")]
    [ProducesResponseType(typeof(List<CommentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetListByIdArticleAsync([FromRoute] int articleId)
    {
        var comments = await _commentService.GetListByIdArticleAsync(articleId);

        return Ok(comments);
    }

    /// <summary>
    /// Get Comment by id
    /// </summary>
    /// <param name="id">Comment id</param>
    /// <returns>Returns CommentDto</returns>
    /// <response code="200">Success</response>
    /// <response code="404">CommentNotFound</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var comment = await _commentService.GetByIdAsync(id);

        return Ok(comment);
    }

    /// <summary>
    /// Create Article
    /// </summary>
    /// <param name="commentInput">CommentInputDto object</param>
    /// <returns>Created CommentDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="404">ArticleNotFound</response>
    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync([FromForm] CommentInputDto commentInput)
    {
        var comment = await _commentService.CreateAsync(commentInput, _currentUserJwtInfo.Result);

        return Ok(comment);
    }

    /// <summary>
    /// Update Comment by id
    /// </summary>
    /// <param name="id">Comment id</param>
    /// <param name="commentInput">CommentInputDto object</param>
    /// <returns>Updated CommentDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="400">NotAccess</response>
    /// <response code="404">CommentNotFound</response>
    [HttpPut ("{id:int}")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int id, [FromForm] CommentInputDto commentInput)
    {
        var comment = await _commentService.UpdateByIdAsync(id, commentInput, _currentUserJwtInfo.Result);

        return Ok(comment);
    }

    /// <summary>
    /// Delete Comment by id
    /// </summary>
    /// <param name="id">Comment id</param>
    /// <response code="200">Success</response>
    /// <response code="400">NotAccess</response>
    /// <response code="404">CommentNotFound</response>
    [HttpDelete ("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
    {
        await _commentService.DeleteByIdAsync(id, _currentUserJwtInfo.Result);

        return Ok();
    }
}