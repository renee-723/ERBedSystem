using ERBedSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ErDbContext>(options => options.UseSqlite("Data Source=ERBedSystem.db"));

builder.Services.AddScoped<ErDbContext>();

builder.Services.AddScoped<ERBedSystem.Repositories.ErRepository>();

builder.Services.AddScoped<ERBedSystem.Services.ErBedService>();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
