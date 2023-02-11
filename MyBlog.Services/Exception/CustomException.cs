using System.Net;

namespace MyBlog.Service.Exception;

public abstract class CustomException : System.Exception
{
    public HttpStatusCode StatusCode { get; }
    public object? ResponseAdditionalInfo { get; }

    protected CustomException(string message, HttpStatusCode statusCode, object? responseAdditionalInfo = null) 
        : base(message)
    {
        StatusCode = statusCode;
        ResponseAdditionalInfo = responseAdditionalInfo;
    }
}