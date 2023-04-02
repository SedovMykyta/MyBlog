using FluentValidation;
using MyBlog.Service.Areas.Articles.AutoMapper.Dto;
using MyBlog.Service.Exception;

namespace MyBlog.Service.Areas.Articles.Validators;

public class ArticleInputDtoValidator: AbstractValidator<ArticleInputDto>
{
    private const int MaxImageSize = 4_194_304;
    
    public ArticleInputDtoValidator()
    {
        RuleFor(article => article.Title)
            .Length(6, 100).WithMessage("Length must be between 6 and 100 characters");
        
        RuleFor(article => article.Description)
            .Length(10, 150).WithMessage("Length must be between 10 and 150 characters");
        
        RuleFor(article => article.FullText)
            .MinimumLength(100).WithMessage("Length must be over 100 characters");

        RuleFor(article => article.Topic)
            .NotEmpty().WithMessage("The Topic field is required");
        
        RuleFor(article => article.Image)
            .Must(image => image == null ? throw new BadRequestException("Image must be uploaded") : true)
            .Must(image => image.FileName[(image.FileName.LastIndexOf('.') + 1)..].ToLower() == "jpg")
            .WithMessage("Uploaded file must be with '.jpg' extension")
            .Must(image => image.Length < MaxImageSize).WithMessage("Upload file must be less than 4mb");
    }
}