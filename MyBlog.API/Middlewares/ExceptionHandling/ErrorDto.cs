namespace MyBlog.Middlewares.ExceptionHandling;

public class ErrorDto
{
    public string ErrorMessage { get; set; }
    
    public object? AdditionalInfo { get; set; }
}