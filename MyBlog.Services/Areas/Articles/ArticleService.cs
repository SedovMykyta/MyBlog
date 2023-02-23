using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.TokenParser.Dto;

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

        ThrowIfEmptyList(articles);
        
        return articles;
    }
    
    public async Task<List<ArticleDto>> GetByUserIdAsync(int userId)
    {
        var articles = await _context.Articles
            .Where(article => article.UserId == userId)
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();
        
        ThrowIfEmptyList(articles);
        
        return articles;
    }

    public async Task<List<ArticleDto>> GetByTopicAsync(Topic topic)
    {
        var articles = await _context.Articles
            .Where(article => article.Topic == topic)
            .Select(article => _mapper.Map<ArticleDto>(article))
            .ToListAsync();

        ThrowIfEmptyList(articles);
        
        return articles;
    }

    public async Task<ArticleDto> GetByIdAsync(int id)
    {
        var article = await GetArticleByIdAsync(id);

        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }
    
    public async Task<ArticleDto> CreateAsync(JwtInfoDto userToken, ArticleDtoInput articleInput)
    {
        await ThrowIfTitleExistAsync(articleInput);
        
        var article = _mapper.Map<Article>(articleInput);

        article.UserId = userToken.Id;
        
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();

        var articleDto = _mapper.Map<ArticleDto>(article);
        
        return articleDto;
    }
    
    public async Task<ArticleDto> UpdateByIdAsync(int id, JwtInfoDto userToken, ArticleDtoInput articleInput)
    {
        var article = await GetArticleByIdAsync(id);

        ThrowIfUserHasNotEditAccess(article.UserId, userToken);
        await ThrowIfTitleExistAsync(articleInput, id);
        
        _mapper.Map(articleInput, article);

        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
        
        var articleDto = _mapper.Map<ArticleDto>(article);

        return articleDto;
    }
    
    public async Task DeleteByIdAsync(int id, JwtInfoDto userToken)
    {
        var article = await GetArticleByIdAsync(id);

        ThrowIfUserHasNotEditAccess(article.UserId, userToken);
        
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
    }


    private async Task<Article> GetArticleByIdAsync(int id)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(article => article.Id == id) 
                      ?? throw new NotFoundException($"Article with Id: {id} is not found");

        return article;
    }

    private void ThrowIfEmptyList<T>(IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new NotFoundException("Articles not found");
        }
    }

    private async Task ThrowIfTitleExistAsync(ArticleDtoInput articleInput)
    {
        if (await _context.Articles.AnyAsync(article => article.Title == articleInput.Title))
        {
            throw new BadRequestException($"Article with Title: {articleInput.Title} exists");
        }
    }
    
    private async Task ThrowIfTitleExistAsync(ArticleDtoInput articleInput, int id)
    {
        var existArticle = await GetArticleByIdAsync(id);

        bool isExistTitle = await _context.Articles.AnyAsync(article =>
            article.Title == articleInput.Title && article.Id != existArticle.Id);
        if (isExistTitle)
        {
            throw new BadRequestException($"Article with Title: {articleInput.Title} exists");
        }
    }
    
    private void ThrowIfUserHasNotEditAccess(int articleUserId, JwtInfoDto userToken)
    {
        if (articleUserId != userToken.Id || userToken.Role != "Admin")
        {
            throw new BadRequestException("You can`t delete this article");
        };
    }
}