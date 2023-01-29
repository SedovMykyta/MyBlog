using System.Net;

namespace MyBlog.Service.Exception;

public abstract class CustomException : System.Exception
{
    public HttpStatusCode StatusCode { get; }

    protected CustomException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}