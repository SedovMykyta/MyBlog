using System.Net;

namespace MyBlog.Service.Exception;

public class NotFoundException : CustomException
{
    private new const HttpStatusCode StatusCode = HttpStatusCode.NotFound;
    
    public NotFoundException(string message) : base(message, StatusCode)
    {
    }
}