using FluentValidation;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Articles.Validators;

public class ArticleInputDtoValidator: AbstractValidator<ArticleInputDto>
{
    private const int MaxSizeImage = 4_194_304;
    
    public ArticleInputDtoValidator()
    {
        RuleFor(article => article.Title)
            .Length(6, 100).WithMessage("Length must be between 6 and 100 characters");
        
        RuleFor(article => article.Description)
            .Length(10, 150).WithMessage("Length must be between 10 and 150 characters");
        
        RuleFor(article => article.FullText)
            .MinimumLength(100).WithMessage("Length must be 100 characters and more");

        RuleFor(article => article.Topic)
            .NotEmpty().WithMessage("The Topic field is required");
        
        RuleFor(article => article.Image)
            .Must(image => image == null ? throw new BadRequestException("Image must be uploaded"): true )
            .Must(image => image.FileName.Substring(image.FileName.LastIndexOf('.') + 1).ToLower() == "jpg")
            .WithMessage("Upload file must have a 'jpg' extension")
            .Must(image => image.Length < MaxSizeImage).WithMessage("Upload file must be less than 4mb");
    }
}