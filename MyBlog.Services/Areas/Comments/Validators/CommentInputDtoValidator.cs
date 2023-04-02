using FluentValidation;
using MyBlog.Service.Areas.Comments.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Comments.Validators;

public class CommentInputDtoValidator: AbstractValidator<CommentInputDto>
{
    public CommentInputDtoValidator()
    {
        RuleFor(comment => comment.Data)
            .Length(2, 150).WithMessage("Length must be between 2 and 150 characters");
    }
}