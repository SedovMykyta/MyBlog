using System.Security.Claims;
using MyBlog.Service.Helpers.TokenParser.Dto;
using static System.Int32;

namespace MyBlog.Service.Helpers.TokenParser;

public class JwtTokenParser : IJwtTokenParser
{
    public JwtInfoDto ParseClaimsIdentityToJwtInfo(ClaimsIdentity? identity)
    {
        if (identity == null)
        {
            throw new UnauthorizedAccessException();
        }
            
        JwtInfoDto jwtInfo = new()
        {
            Id = Parse(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value!),
            Role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value!
        };
        return jwtInfo;
    }
}