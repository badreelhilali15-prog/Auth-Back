using Auth_Back.Data;
using Auth_Back.Models;
using Auth_Back.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Auth_Back.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Configuracion de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//Conexion a la base 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { Roles.Admin, Roles.Client, Roles.Freelancer };

    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);

        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
