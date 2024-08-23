using MemoHubBackend.Data;
using MemoHubBackend.Services;  // Import the Services namespace
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
builder.Services.AddDbContext<MemoHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<AuthService>(); // Register AuthService
builder.Services.AddScoped<UserService>(); // Register UserService
builder.Services.AddScoped<TableService>(); // Register TableService
builder.Services.AddScoped<TodoService>(); // Register TodoService
builder.Services.AddScoped<SubnoteService>(); // Register SubnoteService
builder.Services.AddScoped<DescriptionService>(); // Register DescriptionService

// Add JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "y/mgdPS95NVbwXS6dY6kshS0zjcNgWvXWM7wDu7YeZsd/+dLQ+wzxw7rnEMATGF3f3Y5HNm3A+EqabTyGvjK0sRjE/ikjHdp0rw7y7gxmPPxoV7+P5kzQHXj8JluPCXqt/YN9V3zEjYttu+DYibgIm37NXiidXWvedpwWNSQeAj6TOQ0WhEojAQzXUn1z/huVUf8ASCDzqRSw9vKnS5IC/TqQA1gv4lVD9u8J59ulUzHHHZbP7JWIWQhTU4eUcaJNuGkcRi5tMEPrApwS6Ao8O35h8fn6u3/BsKSwlWOM46dTqXWCWENnzzdxr2NEBu8yF2WPiBJmkV4wCq4F7ICJAzaeO6h2soNtHLwOFYKY7A=");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
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

app.UseHttpsRedirection();

// Add Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
