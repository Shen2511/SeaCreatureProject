using Microsoft.EntityFrameworkCore;
using SeaCreatureApi.Data;
using SeaCreatureApi.Models;

var builder = WebApplication.CreateBuilder(args);


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


var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrEmpty(databaseUrl))
{
   
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(databaseUrl));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.Migrate();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();


var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
