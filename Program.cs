using HomeAssignment.Data;
using HomeAssignment.Services.AssignmentService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

// DB Connection.
var GetConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(GetConnectionString, ServerVersion.AutoDetect(GetConnectionString)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

//Felt like this is too much so gave up on it. Too much of a hassle imo.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters{
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey
//                 (System.Text.Encoding.UTF8.GetBytes
//                 (builder.Configuration.GetSection("AppSettings:Token").Value!)),
//                 ValidateIssuer = false,
//                 ValidateAudience = false
//         };
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
