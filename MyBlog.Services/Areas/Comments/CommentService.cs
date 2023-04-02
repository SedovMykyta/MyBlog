using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Comments.AutoMapper.Dto;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ClaimParser.Dto;
using static MyBlog.Service.ServiceUtilities;

namespace MyBlog.Service.Areas.Comments;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly MyBlogContext _context;

    public CommentService(IMapper mapper, MyBlogContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<CommentDto>> GetListAsync()
    {
        var comments = await _context.Comments
            .Select(comment => _mapper.Map<CommentDto>(comment))
            .ToListAsync();

        return comments;
    }

    public async Task<List<CommentDto>> GetListByArticleIdAsync(int articleId)
    {
        var comments = await _context.Comments
            .Where(comment => comment.ArticleId == articleId)
            .Select(comment => _mapper.Map<CommentDto>(comment))
            .ToListAsync();

        return comments;
    }

    public async Task<CommentDto> GetByIdAsync(int id)
    {
        var comment = await GetCommentByIdAsync(id);

        var commentDto = _mapper.Map<CommentDto>(comment);

        return commentDto;
    }

    public async Task<CommentDto> CreateAsync(CommentInputDto commentInput, JwtInfoDto userToken)
    {
        await ThrowIfArticleNotFound(commentInput.ArticleId);
        
        var comment = _mapper.Map<Comment>(commentInput);

        comment.UserId = userToken.UserId;

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var commentDto = _mapper.Map<CommentDto>(comment);

        return commentDto;
    }

    public async Task<CommentDto> UpdateByIdAsync(int id, CommentInputDto commentInput, JwtInfoDto userToken)
    {
        var comment = await GetCommentByIdAsync(id);
        
        ThrowIfUserCannotEditAccess(comment.UserId, userToken);

        _mapper.Map(commentInput, comment);

        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        var commentDto = _mapper.Map<CommentDto>(comment);

        return commentDto;
    }

    public async Task DeleteByIdAsync(int id, JwtInfoDto userToken)
    {
        var comment = await GetCommentByIdAsync(id);
        
        ThrowIfUserCannotEditAccess(comment.UserId, userToken);

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }


    private async Task<Comment> GetCommentByIdAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id)
                      ?? throw new NotFoundException($"Comment with Id: {id} not found");

        return comment;
    }

    private async Task ThrowIfArticleNotFound(int articleId)
    {
        if (! await _context.Articles.AnyAsync(article => article.Id == articleId))
        {
            throw new NotFoundException($"Article with Id: {articleId} is not found");
        }
    }
}
