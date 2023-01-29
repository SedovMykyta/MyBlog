using Microsoft.EntityFrameworkCore;
using MyBlog.Infrastructure;
using MyBlog.Middlewares.ExceptionHandling;
using MyBlog.Service.Areas.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<MyBlogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogDatabase")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();