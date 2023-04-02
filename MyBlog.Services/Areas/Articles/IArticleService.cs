using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Service.Areas.Articles;

public interface IArticleService
{
    public Task<List<ArticleDto>> GetListAsync();

    public Task<ArticleDto> GetByIdAsync(int id);

    public Task<List<ArticleDto>> GetByUserIdAsync(int userId);

    public Task<List<ArticleDto>> GetByTopicAsync(Topic topic);

    public Task<List<ArticleDto>> GetByTitleAsync(string title);

    public Task<ArticleDto> CreateAsync(ArticleInputDto articleInput, JwtInfoDto userToken);
    
    public Task<ArticleDto> UpdateByIdAsync(int id, ArticleInputDto articleInput, JwtInfoDto userToken);

    public Task DeleteByIdAsync(int id, JwtInfoDto userToken);
}