using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public Role Role { get; set; }

    public DateTime DateCreateAccount { get; set; }

    public List<Article> Articles { get; set; }
}