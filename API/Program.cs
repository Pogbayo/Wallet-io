using Application.Common.Models;
using Application.Interfaces.RepoInterfaces;
using Application.Interfaces.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using SpagWallet.Application.Services;
using SpagWallet.Infrastructure.Persistence.Data;
using SpagWallet.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text; 
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT Key not configured.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false, 
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
    });

app.UseAuthentication();
app.UseAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



app.UseHttpsRedirection();
app.Run();

