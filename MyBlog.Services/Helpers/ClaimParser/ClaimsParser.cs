using System.Security.Claims;
using MyBlog.Service.Helpers.ClaimParser.Dto;
using static System.Int32;

namespace MyBlog.Service.Helpers.ClaimParser;

public class ClaimsParser : IClaimsParser
{
    public JwtInfoDto ToJwtInfo(ClaimsIdentity? identity)
    {
        return identity == null ? throw new UnauthorizedAccessException() : new JwtInfoDto
        {
            Id = Parse(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value!),
            Role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value!
        };
    }
}