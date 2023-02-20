using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Helpers.TokenParser.Dto;

namespace MyBlog.Service.Areas.Articles;

public interface IArticleService
{
    public Task<List<ArticleDto>> GetListAsync();
    
    public Task<List<ArticleDto>> GetByUserIdAsync(int userId);
    
    public Task<ArticleDto> GetByIdAsync(int id);
    
    public Task<List<ArticleDto>> GetByTopicAsync(Topic topic);

    public Task<ArticleDto> CreateAsync(JWTInfo userToken, ArticleDtoInput articleInput);
    
    public Task<ArticleDto> UpdateByIdAsync(int id, JWTInfo userToken, ArticleDtoInput articleInput);

    public Task DeleteByIdAsync(int id, JWTInfo userToken);
}