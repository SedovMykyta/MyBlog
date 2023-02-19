using System.Security.Claims;
using MyBlog.Service.Helpers.ParsingTokens.Dto;

namespace MyBlog.Service.Helpers.ParsingTokens;

public interface IParseJwtToken
{
    public JWTInfo ParseClaimsIdentityToJwtInfo(ClaimsIdentity? identity);
}