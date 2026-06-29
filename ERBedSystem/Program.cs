using ERBedSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ErDbContext>(options => options.UseSqlite("Data Source=ERBedSystem.db"));

builder.Services.AddScoped<ErDbContext>();

builder.Services.AddScoped<ERBedSystem.Repositories.ErRepository>();

builder.Services.AddScoped<ERBedSystem.Services.ErBedService>();

builder.Services.AddEndpointsApiExplorer(); // 確保 Swagger 能掃描到API
builder.Services.AddSwaggerGen(); //加入swagger

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();
app.MapControllers();

app.Run();