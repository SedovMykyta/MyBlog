using Microsoft.AspNetCore.Http;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Service;

public static class ServiceUtilities
{
    public static void ThrowIfUserCannotEditAccess(int? userId, JwtInfoDto userToken)
    {
        if (userId != userToken.UserId && userToken.Role != Role.Admin)
        {
            throw new BadRequestException("You can`t access to this comment");
        }
    }
    
    public static Role CastStringToRole(string inputRole)
    {
        Enum.TryParse(inputRole, true, out Role userRole);

        return userRole;
    }
    
    public static string ToBase64String(this IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        var base64String = Convert.ToBase64String(memoryStream.ToArray());
        
        return base64String;
    } 
}