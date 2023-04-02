using System.Net;

namespace MyBlog.Service.Exception;

public class BadRequestException : CustomException
{
    private new const HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
    
    public BadRequestException(string message) : base(message, StatusCode)
    {
    }
}