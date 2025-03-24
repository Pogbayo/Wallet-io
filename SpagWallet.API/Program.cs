using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpagWallet.Infrastructure.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();



app.UseHttpsRedirection();
app.Run();

