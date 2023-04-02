# My Blog

This project is about people's blogs. It helps to find an article on a topic of interest or to write. Here you can 
create, modify, delete, or find an article. Also contains: authorization, writing comments, rating articles, sending 
emails to subscribed users.

## What's inside

### Web Api  (Backend)
Web Api project contains all necessary endpoints to use the backend application.  
It can be used if you want to create your own frontend application.

## Installation

### What must be installed
- .NET 7
- Microsoft SQL Server
  - Created database

### How to install and run
1) Download/Clone the solution from repository
    - If download make sure to have the solution directory unarchived
2) Open solution directory in terminal
3) Run command:  
   `dotnet user-secrets set "ConnectionStrings:MyBlogDatabase" "<your_connection_string>" -p <startup_project>`  
   where:
    - `<your_connection_string>` - Connection String to your Microsoft SQL Server database
    - `<startup_project>` - Naming of the startup project you want to run. Watch possible values below
4) Be sure your database is run
5) Run command:  
   `dotnet run --project <startup_project>`  
   where `<startup_project>` - Naming of the startup project you want to run. Watch possible values below

> `<startup_project>` possible values:
> - `MyBlog.Api` - Web Api (backend)

## What technologies were used

- .NET 7 + ASP.NET Core
- Microsoft SQL Server
- Entity Framework Core
- Fluent Validator
- AutoMapper
- Jwt Tokens/Authentication
- MailKit
- Swagger
