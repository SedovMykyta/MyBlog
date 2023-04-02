namespace MyBlog.Service.Areas.Rating;

public interface IRatingService
{
    public Task LikeByIdAsync(int articleId, int userId);

    public Task DislikeByIdAsync(int articleId, int userId);
}