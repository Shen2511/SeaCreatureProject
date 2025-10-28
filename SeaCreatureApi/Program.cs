using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data;
using SeaCreatureApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. НАЛАШТУВАННЯ CORS (без змін) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("https://seacreaturess.netlify.app")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// --- 2. "РОЗУМНЕ" ПІДКЛЮЧЕННЯ ДО БАЗИ ДАНИХ ---
// Перевіряємо, чи ми на Railway (чи є змінна DATABASE_URL)
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Так, ми на Railway -> Використовуємо PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(databaseUrl));
}
else
{
    // Ні, ми локально -> Використовуємо SQLite з appsettings.json
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
// --- КІНЕЦЬ ЗМІНИ ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 3. АВТОМАТИЧНА МІГРАЦІЯ ТА СІДІНГ ---
// Цей код тепер безпечний і для SQLite, і для PostgreSQL
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Застосовуємо міграції
    context.Database.Migrate();

    // Додаємо тестові дані (Seeding), якщо база порожня
    if (!context.SeaCreatures.Any())
    {
        context.SeaCreatures.AddRange(
            new SeaCreature { Name = "Great White Shark", Lifespan = 30, DietType = "Carnivore", Habitat = "Atlantic Ocean" },
            new SeaCreature { Name = "Sea Turtle", Lifespan = 80, DietType = "Herbivore", Habitat = "Pacific Ocean" },
            new SeaCreature { Name = "Clownfish", Lifespan = 6, DietType = "Omnivore", Habitat = "Indian Ocean" }
        );
        context.SaveChanges();
    }
}
// --- КІНЕЦЬ ЗМІНИ ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();