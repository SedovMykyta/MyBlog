using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Helpers.ParsingTokens.Dto;

namespace MyBlog.Service.Areas.Articles;

public interface IArticleService
{
    public Task<List<ArticleDto>> GetListAsync();
    
    public Task<List<ArticleDto>> GetByUserIdAsync(int userId);
    
    public Task<ArticleDto> GetByIdAsync(int id);
    
    public Task<List<ArticleDto>> GetByThemeAsync(Topic topic);

    public Task<ArticleDto> CreateAsync(JWTInfo userJwtInfo, ArticleDtoInput articleInput);
    
    public Task<ArticleDto> UpdateByIdAsync(int id, JWTInfo userJwtInfo, ArticleDtoInput articleInput);

    public Task DeleteByIdAsync(int id, JWTInfo userJwtInfo);
}