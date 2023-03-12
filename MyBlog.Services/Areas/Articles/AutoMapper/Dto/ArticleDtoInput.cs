using Microsoft.AspNetCore.Http;
using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Service.Areas.Articles.AutoMapper.Dto;

public class ArticleDtoInput
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string FullText { get; set; }
    public Topic Topic { get; set; }
    public IFormFile Image { get; set; }
}