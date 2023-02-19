using FluentValidation;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;

namespace MyBlog.Service.Areas.Articles.Validators;

public class ArticleDtoInputValidator: AbstractValidator<ArticleDtoInput>
{
    public ArticleDtoInputValidator()
    {
        RuleFor(article => article.Title)
            .Length(6, 100).WithMessage("Length must be between 6 characters and 100");
        
        RuleFor(article => article.Description)
            .Length(10, 150).WithMessage("Length must be between 10 characters and 150");
        
        RuleFor(article => article.FullText)
            .MinimumLength(100).WithMessage("Length must be 100 characters and more");
    }
}