using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Service.Exception;

namespace MyBlog.Filtres;
public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (! context.ModelState.IsValid)
        {
            var fieldErrors = context.ModelState
                .Where(kvp => kvp.Value!.Errors.Count > 0)
                .ToDictionary(key => key.Key,
                    value => string.Join(". ", value.Value!.Errors.Select(modelerror => modelerror.ErrorMessage)));
            
            throw new ValidationErrorException(fieldErrors);
        }
    
        await next();
    }
}