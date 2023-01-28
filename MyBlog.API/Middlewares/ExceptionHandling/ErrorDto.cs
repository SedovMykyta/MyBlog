using System.Text.Json;

namespace MyBlog.Middlewares.ExceptionHandling;

public class ErrorDto
{
    public string? ErrorMessage { get; set; }
}