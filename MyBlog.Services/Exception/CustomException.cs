using System.Net;

namespace MyBlog.Service.Exception;

public abstract class CustomException : System.Exception
{
    public HttpStatusCode ResponseStatusCode { get; }

    protected CustomException(string message, HttpStatusCode responseStatusCode) 
        : base(message)
    {
        ResponseStatusCode = responseStatusCode;
    }
}