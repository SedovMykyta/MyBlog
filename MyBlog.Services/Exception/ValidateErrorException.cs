using System.Net;

namespace MyBlog.Service.Exception;

public class ValidationErrorException : CustomException
{
    private const string ValidationErrorMessage = "Input data has not passed the validation";
    private const HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
    
    public ValidationErrorException(Dictionary<string, string> fieldErrors)
        : base(ValidationErrorMessage, StatusCode, fieldErrors)
    {
    }
}