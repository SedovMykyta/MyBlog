using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Role Role { get; set; }
    public DateTime DateCreateAccount { get; set; } = DateTime.UtcNow;

    public IList<Article>? Articles { get; set; } 
    public IList<Comment> Comments { get; set; }
    public IList<Like> Likes { get; set; }
    public IList<Dislike> Dislikes { get; set; } 
}
