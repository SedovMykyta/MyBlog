using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ClaimParser.Dto;
using MyBlog.Service.Helpers.ExtensionMethods;

namespace MyBlog.Service.Areas.Articles;

public class ArticleService : IArticleService
{
    private readonly MyBlogContext _context;
    private readonly IMapper _mapper;

    public ArticleService(IMapper mapper, MyBlogContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<ArticleDto>> GetListAsync()
    {
        var articles = await _context.Articles
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();

        return articles;
    }

    public async Task<ArticleDto> GetByIdAsync(int id)
    {
        var article = await GetArticleByIdAsync(id);

        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }

    public async Task<List<ArticleDto>> GetByUserIdAsync(int userId)
    {
        var articles = await _context.Articles
            .Where(article => article.UserId == userId)
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();

        return articles;
    }

    public async Task<List<ArticleDto>> GetByTopicAsync(Topic topic)
    {
        var articles = await _context.Articles
            .Where(article => article.Topic == topic)
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();

        return articles;
    }

    public async Task<List<ArticleDto>> GetByTitleAsync(string title)
    {
        var articles = await _context.Articles
            .Where(article => article.Title.Contains(title) || title.Contains(article.Title))
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();

        return articles;
    }

    public async Task<ArticleDto> CreateAsync(ArticleDtoInput articleInput, JwtInfoDto userToken)
    {
        await ThrowIfTitleExistAsync(articleInput.Title);
        
        var article = _mapper.Map<Article>(articleInput);

        article.UserId = userToken.Id;
        
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();

        var articleDto = _mapper.Map<ArticleDto>(article);
        
        return articleDto;
    }
    
    public async Task<ArticleDto> UpdateByIdAsync(int id, ArticleDtoInput articleInput, JwtInfoDto userToken)
    {
        var article = await GetArticleByIdAsync(id);

        ThrowIfUserCannotEditAccess(article.UserId, userToken);
        await ThrowIfTitleExistAsync(articleInput.Title, article.Id);
        
        _mapper.Map(articleInput, article);

        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        
        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }
    
    public async Task DeleteByIdAsync(int id, JwtInfoDto userToken)
    {
        var article = await GetArticleByIdAsync(id);

        ThrowIfUserCannotEditAccess(article.UserId, userToken);
        
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
    }


    private async Task<Article> GetArticleByIdAsync(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(article => article.Id == id) 
                      ?? throw new NotFoundException($"Article with Id: {id} not found");

        return article;
    }
    
    private async Task ThrowIfTitleExistAsync(string title, int id = -1)
    {
        if (await _context.Articles.AnyAsync(article => article.Title == title && article.Id != id))
        {
            throw new BadRequestException($"Article with Title: {title} exists");
        }
    }
    
    private void ThrowIfUserCannotEditAccess(int articleUserId, JwtInfoDto userToken)
    {
        if (articleUserId != userToken.Id && userToken.Role != "Admin")
        {
            throw new BadRequestException("You do not have permission");
        };
    }
}