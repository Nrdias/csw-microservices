using Microsoft.EntityFrameworkCore;
using DisciplineService.Data;
using DisciplineService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=disciplines.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

await using var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await db.Database.EnsureCreatedAsync();

// Endpoints
app.MapPost("/disciplines", async (AppDbContext db, Discipline discipline) =>
{
    if (await db.Disciplines.AnyAsync(d => d.Code == discipline.Code && d.Schedule == discipline.Schedule))
        return Results.BadRequest("Discipline with this code and schedule already exists");

    db.Disciplines.Add(discipline);
    await db.SaveChangesAsync();
    return Results.Created($"/disciplines/{discipline.Id}", discipline);
});

app.MapGet("/disciplines/{code}/{schedule}", async (AppDbContext db, string code, char schedule) =>
{
    var discipline = await db.Disciplines.FirstOrDefaultAsync(d => d.Code == code && d.Schedule == schedule);
    return discipline is not null ? Results.Ok(discipline) : Results.NotFound();
});

app.MapGet("/disciplines", async (AppDbContext db) =>
{
    var disciplines = await db.Disciplines.ToListAsync();
    return Results.Ok(disciplines);
});

app.Run();
