namespace MyBlog.Infrastructure.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Data { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; }
    
    public int? UserId { get; set; }
    public User User { get; set; }
    
    public int ArticleId { get; set; } 
    public Article Article { get; set; }
}
