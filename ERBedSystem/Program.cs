using ERBedSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ErDbContext>(options => options.UseSqlite("Data Source=ERBedSystem.db"));

builder.Services.AddScoped<ErDbContext>();

builder.Services.AddScoped<ERBedSystem.Repositories.ErRepository>();

builder.Services.AddScoped<ERBedSystem.Services.ErBedService>();

builder.Services.AddEndpointsApiExplorer(); // 確保 Swagger 能掃描到API
builder.Services.AddSwaggerGen(); //加入swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ErDbContext>();
    db.Database.EnsureCreated();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();