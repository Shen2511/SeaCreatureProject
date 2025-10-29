using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data; // простір імен, де лежить твій DbContext

var builder = WebApplication.CreateBuilder(args);

// Додаємо DbContext з PostgreSQL
builder.Services.AddDbContext<DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додаємо контролери / мінімальні API
builder.Services.AddControllers();

// Swagger (якщо потрібно для тестів)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
