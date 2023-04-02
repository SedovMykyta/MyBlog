namespace MyBlog.Infrastructure.Entities;

public class UserSubscription
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public bool IsSubscribedToEmail { get; set; }
}