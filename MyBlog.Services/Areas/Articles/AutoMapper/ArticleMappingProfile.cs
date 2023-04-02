using AutoMapper;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Articles.AutoMapper;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<ArticleInputDto, Article>()
            .ForMember(article => 
                    article.UpdatedDate, memberOptions => memberOptions.MapFrom(dateUpdated => DateTime.UtcNow))
            .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => src.Image.ToBase64String()));
        
    }
} 