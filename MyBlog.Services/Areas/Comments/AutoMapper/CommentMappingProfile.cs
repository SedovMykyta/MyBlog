using AutoMapper;
using MyBlog.Infrastructure.Entities;
using MyBlog.Service.Areas.Comments.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Comments.AutoMapper;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>();
        CreateMap<CommentInputDto, Comment>()
            .ForMember(comment => comment.DateUpdated,
                memberOptions => memberOptions.MapFrom(dateUpdated => DateTime.UtcNow));

    }
}