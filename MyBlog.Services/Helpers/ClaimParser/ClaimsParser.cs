using System.Security.Claims;
using MyBlog.Service.Helpers.ClaimParser.Dto;
using static System.Int32;
using static MyBlog.Service.ServiceUtilities;

namespace MyBlog.Service.Helpers.ClaimParser;

public class ClaimsParser : IClaimsParser
{
    public JwtInfoDto ToJwtInfo(ClaimsIdentity? identity)
    {
        return identity == null ? throw new UnauthorizedAccessException() : new JwtInfoDto
        {
            UserId = Parse(identity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value!),
            Role = CastStringToRole(identity.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value!) 
        };
    }
}