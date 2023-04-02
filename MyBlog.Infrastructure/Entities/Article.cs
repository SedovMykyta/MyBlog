﻿using MyBlog.Infrastructure.Entities.Enum;

namespace MyBlog.Infrastructure.Entities;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FullText { get; set; }
    public string ImageBase64 { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public Topic Topic { get; set; }
    
    public int UserId { get; set; } 
    public User User { get; set; }
    
    public IList<Comment> Comments { get; set; } 
    public IList<Like> Likes { get; set; } 
    public IList<Dislike> Dislikes { get; set; } 
}
