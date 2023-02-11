using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Filtres;
using MyBlog.Infrastructure;
using MyBlog.Middlewares.ExceptionHandling;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Areas.Users.AutoMapper;
using MyBlog.Service.Areas.Users.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config => config.Filters.Add<ValidationFilter>());

builder.Services.AddDbContext<MyBlogContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogDatabase")));
    
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<UserDtoInputValidator>());
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile));

builder.Services.AddSingleton<UserDtoInputValidator>();

builder.Services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

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