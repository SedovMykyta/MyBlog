using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Articles;

namespace MyBlog.Service.Areas.Rating;

public class RatingService : IRatingService
{
    private readonly MyBlogContext _context;
    private readonly IArticleService _articleService;

    public RatingService(MyBlogContext context, IArticleService articleService)
    {
        _context = context;
        _articleService = articleService;
    }
    
    public async Task LikeByIdAsync(int articleId, int userId)
    {
        await ThrowIfArticleIsNotExist(articleId);

        var like = await GetLikeAsync(articleId, userId);
        if (like != null)
        {
            await RemoveLikeAsync(like);
            return;
        }

        var dislike = await GetDislikeAsync(articleId, userId);
        if (dislike != null)
        {
            await RemoveDislikeAsync(dislike);
        }

        await _context.Likes.AddAsync(new Like { ArticleId = articleId, UserId = userId });
        await _context.SaveChangesAsync();
    }

    public async Task DislikeByIdAsync(int articleId, int userId)
    {
        await ThrowIfArticleIsNotExist(articleId);

        var dislike = await GetDislikeAsync(articleId, userId);
        if (dislike != null)
        {
            await RemoveDislikeAsync(dislike);
            return;
        }

        var like = await GetLikeAsync(articleId, userId);
        if (like != null)
        {
            await RemoveLikeAsync(like);
        }

        await _context.Dislikes.AddAsync(new Dislike { ArticleId = articleId, UserId = userId });
        await _context.SaveChangesAsync();
    }

    private async Task ThrowIfArticleIsNotExist(int id)
    {
        await _articleService.GetByIdAsync(id);
    }

    private async Task<Dislike?> GetDislikeAsync(int articleId, int userId)
    {
        var dislike = await _context.Dislikes.FirstOrDefaultAsync
            (dislike => dislike.ArticleId == articleId && dislike.UserId == userId);

        return dislike;
    }
    
    private async Task<Like?> GetLikeAsync(int articleId, int userId)
    {
        var like = await _context.Likes.
            FirstOrDefaultAsync(like => like.ArticleId == articleId && like.UserId == userId);
        
        return like;
    }

    private async Task RemoveLikeAsync(Like like)
    {
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    private async Task RemoveDislikeAsync(Dislike dislike)
    {
        _context.Dislikes.Remove(dislike);
        await _context.SaveChangesAsync();
    }
}