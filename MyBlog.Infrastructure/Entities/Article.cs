namespace MyBlog.Infrastructure.Entities;

public class Article
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string FullText { get; set; }
}