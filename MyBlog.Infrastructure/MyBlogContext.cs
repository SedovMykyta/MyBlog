using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure.Entities;

namespace MyBlog.Infrastructure;

public class MyBlogContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Dislike> Dislikes { get; set; }
    
    public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
    {
    }
}