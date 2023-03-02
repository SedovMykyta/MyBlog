namespace MyBlog.Infrastructure.Entities;

public class Dislike
{
    public int Id { get; set; } 
    
    public int? UserId { get; set; } 
    public virtual User User { get; set; }
    
    public int? ArticleId { get; set; } 
    public virtual Article Article { get; set; } 
}
