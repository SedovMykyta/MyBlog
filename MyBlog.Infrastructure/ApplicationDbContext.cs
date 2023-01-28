using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure.Entities;

namespace MyBlog.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    

    public DbSet<User> Users { get; set; }
    
    public DbSet<Article> Articles { get; set; }

   
}