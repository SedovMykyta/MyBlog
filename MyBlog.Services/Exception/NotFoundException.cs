using System.Net;

namespace MyBlog.Service.Exception;

public class NotFoundException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.NotFound;
    
    public NotFoundException(string message) 
        : base(message, StatusCode)
    {
    }
}