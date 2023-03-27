# HomeAssignment
Oversight

Need 2 install packages:
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools (?)
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.IdentityModel.Tokens 
System.IdentityModel.Tokens.Jwt
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.4

Need 2 install extensions:
SQLTools
SQLTools MySQL/MariaDB

Need 2 setup old MySql type of auth:
CREATE USER 'sqluser'@'%' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT ALL PRIVILEGES ON *.* TO 'sqluser'@'%';
FLUSH PRIVILEGES;
