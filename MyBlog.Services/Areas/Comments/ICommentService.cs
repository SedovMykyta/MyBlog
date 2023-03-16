using MyBlog.Service.Areas.Comments.AutoMapper.Dto;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Service.Areas.Comments;

public interface ICommentService
{
    public Task<List<CommentDto>> GetListAsync();
    
    public Task<List<CommentDto>> GetListByIdArticleAsync(int articleId);

    public Task<CommentDto> GetByIdAsync(int id);

    public Task<CommentDto> CreateAsync(CommentInputDto commentInput, JwtInfoDto userToken);

    public Task<CommentDto> UpdateByIdAsync(int id, CommentInputDto commentInput, JwtInfoDto userToken);

    public Task DeleteByIdAsync(int id, JwtInfoDto userToken);

}