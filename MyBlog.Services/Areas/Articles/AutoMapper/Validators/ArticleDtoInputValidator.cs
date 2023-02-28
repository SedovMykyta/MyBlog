using FluentValidation;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Articles.AutoMapper.Validators;

public class ArticleDtoInputValidator: AbstractValidator<ArticleDtoInput>
{
    private const long MaxSizeImage = 4_194_304;
    
    public ArticleDtoInputValidator()
    {
        RuleFor(article => article.Title)
            .Length(6, 100).WithMessage("Length must be between 6 characters and 100");
        
        RuleFor(article => article.Description)
            .Length(10, 150).WithMessage("Length must be between 10 characters and 150");
        
        RuleFor(article => article.FullText)
            .MinimumLength(100).WithMessage("Length must be 100 characters and more");

        RuleFor(article => article.Topic)
            .NotEmpty().WithMessage("The Topic field is required");
        
        RuleFor(article => article.Image)
            .Must(image => image == null? throw new BadRequestException("Image must be uploaded"): true )
            .Must(image => image.FileName.Substring(image.FileName.LastIndexOf('.') + 1).ToLower() == "jpg")
            .WithMessage("Upload file must have a 'jpg' extension")
            .Must(image => image.Length < MaxSizeImage).WithMessage("Upload file must be less than 4mb");
    }
}