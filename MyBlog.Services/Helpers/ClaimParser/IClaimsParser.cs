using System.Security.Claims;
using MyBlog.Service.Helpers.ClaimParser.Dto;

namespace MyBlog.Service.Helpers.ClaimParser;

public interface IClaimsParser
{
    public JwtInfoDto ToJwtInfo(ClaimsIdentity? identity);
}