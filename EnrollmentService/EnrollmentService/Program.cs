using Microsoft.EntityFrameworkCore;
using EnrollmentService.Data;
using EnrollmentService.Models;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=enrollments.db"));
builder.Services.AddHttpClient();

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
app.MapPost("/enrollments", async (AppDbContext db, IHttpClientFactory httpClientFactory, EnrollmentRequest request) =>
{
    var client = httpClientFactory.CreateClient();

    // Check student
    var studentResponse = await client.GetAsync($"http://studentservice/students/{request.StudentRegistrationNumber}");
    if (!studentResponse.IsSuccessStatusCode)
        return Results.BadRequest("Student not found");

    // Check discipline
    var disciplineResponse = await client.GetAsync($"http://disciplineservice/disciplines/{request.DisciplineCode}/{request.Schedule}");
    if (!disciplineResponse.IsSuccessStatusCode)
        return Results.BadRequest("Discipline not found");

    // Check if already enrolled
    if (await db.Enrollments.AnyAsync(e => e.StudentRegistrationNumber == request.StudentRegistrationNumber &&
                                           e.DisciplineCode == request.DisciplineCode &&
                                           e.Schedule == request.Schedule))
        return Results.BadRequest("Already enrolled");

    var enrollment = new Enrollment
    {
        StudentRegistrationNumber = request.StudentRegistrationNumber,
        DisciplineCode = request.DisciplineCode,
        Schedule = request.Schedule
    };

    db.Enrollments.Add(enrollment);
    await db.SaveChangesAsync();
    return Results.Created($"/enrollments/{enrollment.Id}", enrollment);
});

app.Run();
