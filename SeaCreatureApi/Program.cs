using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 👇 Жорстке прив’язування Kestrel до порту з Railway
builder.WebHost.ConfigureKestrel(options =>
{
    var port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "5000");
    options.ListenAnyIP(port);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger завжди доступний
app.UseSwagger();
app.UseSwaggerUI();

// 👇 Тимчасово можна вимкнути редірект на HTTPS, щоб уникнути помилок у контейнері
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
