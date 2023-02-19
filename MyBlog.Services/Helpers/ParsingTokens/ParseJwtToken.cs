using System.Security.Claims;
using MyBlog.Service.Helpers.ParsingTokens.Dto;
using static System.Int32;

namespace MyBlog.Service.Helpers.ParsingTokens;

public class ParseJwtToken : IParseJwtToken
{
    public JWTInfo ParseClaimsIdentityToJwtInfo(ClaimsIdentity? identity)
    {
        if (identity == null)
        {
            throw new UnauthorizedAccessException();
        }
            
        JWTInfo jwtInfo = new()
        {
            Id = Parse(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value!),
            Role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
        };
        return jwtInfo;
    }
}