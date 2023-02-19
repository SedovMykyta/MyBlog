using AutoMapper;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Articles.AutoMapper;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<ArticleDtoInput, Article>();
    }
}