namespace MyBlog.Service.Areas.Comments.AutoMapper.Dto;

public class CommentDto
{
    public int Id { get; set; }
    public string Data { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public int? UserId { get; set; }
    public int ArticleId { get; set; }
}