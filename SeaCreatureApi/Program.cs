using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlify",
        policy =>
        {
            policy.WithOrigins("https://seacreaturess.netlify.app") // твій фронтенд-домен
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
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
app.UseCors("AllowNetlify");
// Swagger завжди доступний
app.UseSwagger();
app.UseSwaggerUI();

// 👇 Тимчасово можна вимкнути редірект на HTTPS, щоб уникнути помилок у контейнері
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
