using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=students.db"));

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
app.MapPost("/students", async (AppDbContext db, Student student) =>
{
    if (await db.Students.AnyAsync(s => s.RegistrationNumber == student.RegistrationNumber))
        return Results.BadRequest("Registration number already exists");

    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{student.Id}", student);
});

app.MapGet("/students/{registrationNumber}", async (AppDbContext db, string registrationNumber) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.RegistrationNumber == registrationNumber);
    return student is not null ? Results.Ok(student) : Results.NotFound();
});

app.MapGet("/students/search/{namePart}", async (AppDbContext db, string namePart) =>
{
    var students = await db.Students.Where(s => s.Name.Contains(namePart)).ToListAsync();
    return Results.Ok(students);
});

app.Run();
