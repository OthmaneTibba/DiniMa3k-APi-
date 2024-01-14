using CloudinaryDotNet;
using DiniM3ak.Data;
using DiniM3ak.Entity;
using DiniM3ak.Services;
using DiniM3ak.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthUtils>();

builder.Services.AddCors(policy =>
{
    policy.AddDefaultPolicy(options =>
    {
        options
       .AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod();
       
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data source=app.db");
});


builder.Services.AddSingleton<Cloudinary>(_ => new Cloudinary(new Account(
                builder.Configuration["Cloudinary:CloudName"],
                builder.Configuration["Cloudinary:ApiKey"],
                 builder.Configuration["Cloudinary:ApiSecret"]
            )));
builder.Services.AddScoped<PictureService>();

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
