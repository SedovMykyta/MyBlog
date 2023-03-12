using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Service.Exception;

namespace MyBlog.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (! context.ModelState.IsValid)
        {
            var fieldErrors = context.ModelState
                .Where(keyValuePair => keyValuePair.Value!.Errors.Count > 0)
                .ToDictionary(key => key.Key,
                    value => string.Join(". ", value.Value!.Errors.Select(modelError => modelError.ErrorMessage)));
            
            throw new ValidationErrorException(fieldErrors);
        }
    
        await next();
    }
}