namespace MyBlog.Service.Areas.Comments.AutoMapper.Dto;

public class CommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    
    public int? UserId { get; set; }
    public int ArticleId { get; set; }
}