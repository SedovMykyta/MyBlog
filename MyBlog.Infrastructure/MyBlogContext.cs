using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure.Entities;

namespace MyBlog.Infrastructure;

public class MyBlogContext : DbContext
{
    public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Article> Articles { get; set; }

   
}