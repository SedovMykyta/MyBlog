using System.Security.Claims;
using MyBlog.Service.Helpers.TokenParser.Dto;

namespace MyBlog.Service.Helpers.TokenParser;

public interface IJwtTokenParser
{
    public JwtInfoDto ParseClaimsIdentityToJwtInfo(ClaimsIdentity? identity);
}