using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.TokenParser.Dto;

namespace MyBlog.Service.Areas.Articles;

public class ArticleService : IArticleService
{
    private readonly MyBlogContext _context;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ArticleService(IMapper mapper, MyBlogContext context, IUserService userService)
    {
        _mapper = mapper;
        _context = context;
        _userService = userService;
    }

    public async Task<List<ArticleDto>> GetListAsync()
    {
        var articles = await _context.Articles
            .Select(a => _mapper.Map<ArticleDto>(a))
            .ToListAsync();

        return articles;
    }
    
    public async Task<List<ArticleDto>> GetByUserIdAsync(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException($"User with Id: {userId} is not found");
        }

        var articles = await _context.Articles
            .Where(a => a.UserId == userId)
            .Select(a => _mapper.Map<ArticleDto>(a))
            .ToListAsync();
        
        return articles;
    }

    public async Task<List<ArticleDto>> GetByThemeAsync(Topic topic)
    {
        var articles = await _context.Articles
            .Where(a => a.Topic == topic)
            .Select(a => _mapper.Map<ArticleDto>(a))
            .ToListAsync();

        return articles;
    }

    public async Task<ArticleDto> GetByIdAsync(int id)
    {
        var article = await GetArticleByIdAsync(id);

        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }
    
    public async Task<ArticleDto> CreateAsync(JWTInfo userJwtInfo, ArticleDtoInput articleInput)
    {
        if (await _context.Articles.AnyAsync(a => a.Title == articleInput.Title))
        {
            throw new BadRequestException($"Article with Title: {articleInput.Title} exists");
        }
        
        var article = _mapper.Map<Article>(articleInput);

        article.UserId = userJwtInfo.Id;
        
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();

        var articleDto = _mapper.Map<ArticleDto>(article);
        
        return articleDto;
    }
    
    public async Task<ArticleDto> UpdateByIdAsync(int id, JWTInfo userJwtInfo, ArticleDtoInput articleInput)
    {
        var article = await GetArticleByIdAsync(id);
        
        if (await CheckTitleAreBusyAsync(id, articleInput))
        {
            throw new BadRequestException($"Article with Title: {articleInput.Title} exists");
        }

        if ( ! HaveTheRightOnDeleteOrUpdate(article, userJwtInfo))
        {
            throw new BadRequestException("You don`t delete this article");
        }
        
        _mapper.Map(articleInput, article);

        article.DateLastChangedArticle = DateTime.UtcNow;

        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        
        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }
    
    public async Task DeleteByIdAsync(int id, JWTInfo userJwtInfo)
    {
        var article = await GetArticleByIdAsync(id);

        if (! HaveTheRightOnDeleteOrUpdate(article, userJwtInfo))
        {
            throw new BadRequestException("You don`t delete this article");
        }
        
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> CheckTitleAreBusyAsync(int id, ArticleDtoInput articleInput)
    {
        var article = await GetArticleByIdAsync(id);

        return await _context.Articles.AnyAsync(a => a.Title == articleInput.Title && a.Id != article.Id);
    }
    
    private async Task<Article> GetArticleByIdAsync(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id) 
                      ?? throw new NotFoundException($"Article with Id: {id} is not found");

        return article;
    }
    private bool HaveTheRightOnDeleteOrUpdate(Article article, JWTInfo userJwtInfo)
    {
        return article.UserId == userJwtInfo.Id || userJwtInfo.Role == "Admin";
    }
}