using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyBlog.Filtres;
using MyBlog.Infrastructure;
using MyBlog.Middlewares.ExceptionHandling;
using MyBlog.Service.Areas.Articles;
using MyBlog.Service.Areas.Articles.AutoMapper;
using MyBlog.Service.Areas.Auth;
using MyBlog.Service.Areas.Users;
using MyBlog.Service.Areas.Users.AutoMapper;
using MyBlog.Service.Areas.Users.Validators;
using MyBlog.Service.Helpers.PasswordManagers;
using MyBlog.Service.Helpers.ClaimParser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(config => config.Filters.Add<ValidationFilter>());

builder.Services.AddDbContext<MyBlogContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogDatabase")));

builder.Services.AddValidatorsFromAssemblyContaining<UserDtoInputValidator>();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)
    .AddFluentValidationAutoValidation(); 
builder.Services.AddFluentValidationClientsideAdapters(); 

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPasswordManager, PasswordManager>();
builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddTransient<IClaimsParser, ClaimsParser>();

builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(ArticleMappingProfile));

builder.Services.AddSingleton<UserDtoInputValidator>();

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(config =>
{
    config.SchemaFilter<EnumSchemaFilter>();
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
    
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>() 
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(config =>
{
    config.AllowAnyOrigin();
    config.AllowAnyHeader();
    config.AllowAnyMethod();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();