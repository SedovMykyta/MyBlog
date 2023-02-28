using Microsoft.AspNetCore.Http;

namespace MyBlog.Service.Helpers.ExtensionMethods;

public static class FormFileExtensions
{
    public static string ToBase64String(this IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        var base64String = Convert.ToBase64String(memoryStream.ToArray());
        
        return base64String;
    } 
}