namespace MyBlog.Infrastructure.Entities;

public class Like
{
    public int Id { get; set; } 
    
    public int? UserId { get; set; } 
    public virtual User User { get; set; } 
    
    public int ArticleId { get; set; } 
    public Article Article { get; set; }
}
