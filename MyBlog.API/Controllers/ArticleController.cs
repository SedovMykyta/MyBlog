using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Helpers.ParsingTokens;
using MyBlog.Service.Helpers.ParsingTokens.Dto;

namespace MyBlog.Controllers;

[Route("api/article")]
[ApiController]
[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class ArticleController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IArticleService _articleService;
    private readonly Task<JWTInfo> _currentUserJwtInfo;
    public ArticleController(ILogger<UserController> logger, IArticleService articleService, 
        IParseJwtToken parseJwtToken)
    {
        _logger = logger;
        _articleService = articleService;
        _currentUserJwtInfo = Task.Run(() => 
            parseJwtToken.ParseClaimsIdentityToJwtInfo(HttpContext.User.Identity as ClaimsIdentity));
    }


    /// <summary>
    /// Get list of Articles
    /// </summary>
    /// <returns>Returns ArticleDto list</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ArticleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetListAsync()
    {
        var articles = await _articleService.GetListAsync();
        return Ok(articles);
    }


    /// <summary>
    /// Get Articles by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <returns>Returns ArticlesDto</returns>
    /// <response code="200">Success</response>
    [HttpGet("get-by-user-id{userId:int}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute] int userId)
    {
        var articles = await _articleService.GetByUserIdAsync(userId);

        return Ok(articles);
    }
    
    /// <summary>
    /// Get Articles by theme
    /// </summary>
    /// <param name="topic">Theme article</param>
    /// <returns>Returns ArticlesDto</returns>
    /// <response code="200">Success</response>
    [HttpGet("get-by-theme{topic}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByThemeAsync([FromRoute] Topic topic)
    {
        var articles = await _articleService.GetByThemeAsync(topic);
        
        return Ok(articles);
    }
    
    /// <summary>
    /// Get Article by id
    /// </summary>
    /// <param name="id">Article id</param>
    /// <returns>Returns ArticleDto</returns>
    /// <response code="200">Success</response>
    /// <response code="404">ArticleNotFound</response>
    [HttpGet("get-by-id{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticleByIdAsync([FromRoute] int id)
    {
        var article = await _articleService.GetByIdAsync(id);

        return Ok(article);
    }

    /// <summary>
    /// Create Article
    /// </summary>
    /// <param name="articleInput">ArticleDtoInput object</param>
    /// <returns>Created ArticleDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="409">TitleExists</response>
    [HttpPost]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] ArticleDtoInput articleInput)
    {
        var article = await _articleService.CreateAsync(_currentUserJwtInfo.Result, articleInput);

        return Ok(article);
    }

    /// <summary>
    /// Update Article by id
    /// </summary>
    /// <param name="articleId">Article id</param>
    /// <param name="articleInput">ArticleDtoInput object</param>
    /// <returns>Updated ArticleDto object</returns>
    /// <response code="200">Success</response>
    /// <response code="404">UserNotFound</response>
    /// <response code="409">TitleExists</response>
    [HttpPut ("{articleId:int}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateByIdAsync([FromRoute] int articleId, [FromBody] ArticleDtoInput articleInput)
    {
        var article = await _articleService.UpdateByIdAsync(articleId, _currentUserJwtInfo.Result, articleInput);

        return Ok(article);
    }

    /// <summary>
    /// Delete Article by id
    /// </summary>
    /// <param name="articleId">Article id</param>
    /// <response code="200">Success</response>
    /// <response code="404">ArticleNotFound</response>
    [HttpDelete ("{articleId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int articleId)
    {
        await _articleService.DeleteByIdAsync(articleId, _currentUserJwtInfo.Result);

        return Ok();
    }
}