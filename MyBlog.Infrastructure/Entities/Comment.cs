namespace MyBlog.Infrastructure.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    
    public int? UserId { get; set; }
    public virtual User User { get; set; }
    
    public int? ArticleId { get; set; } 
    public virtual Article Article { get; set; }
}
