using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Service.Helpers.ClaimParser.Dto;

public class JwtInfoDto
{
    public int UserId { get; set; }
    public Role Role { get; set; }
}