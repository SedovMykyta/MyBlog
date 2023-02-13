using System.Net;

namespace MyBlog.Service.Exception;

public class ExistsException : CustomException
{
    private const HttpStatusCode StatusCode = HttpStatusCode.Conflict;
    
    public ExistsException(string message) : base(message, StatusCode)
    {
    }
}