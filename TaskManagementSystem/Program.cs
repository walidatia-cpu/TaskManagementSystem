using Microsoft.AspNetCore.Builder;
using TaskManagementSystem.Extensions;
using TaskManagementSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // OpenAPI configuration

// Register services using extension methods
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerWithJwt(); // Added Swagger with JWT Support
var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
// Apply migrations and seed data using the extension method
app.ApplyMigrationsAndSeedData();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.Run();
