using FluentValidation;
using MyBlog.Service.Areas.Comments.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Comments.Validators;

public class CommentInputDtoValidator: AbstractValidator<CommentInputDto>
{
    public CommentInputDtoValidator()
    {
        RuleFor(comment => comment.Text)
            .Length(2, 150).WithMessage("Length must be between 2 characters and 150");

        RuleFor(comment => comment.ArticleId)
            .Must(id => int.TryParse(id.ToString(), out _) == false
                ? throw new BadRequestException("You need entering the number") : true);

    }
}