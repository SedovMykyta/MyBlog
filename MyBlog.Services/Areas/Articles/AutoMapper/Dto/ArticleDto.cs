using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Service.Areas.Articles.AutoMapper.Dto;

public class ArticleDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string FullText { get; set; }

    public DateTime DateCreate { get; set; } 

    public DateTime DateUpdated { get; set; } 
    
    public Topic Topic { get; set; }
}