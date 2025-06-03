using ICHomePageDataMicroservice.Configuration;
using ICHomePageDataMicroservice.Repositories;
using ICHomePageDataMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ICHomePageData API",
        Version = "v1",
        Description = "API for GlobalData Intelligence Center Homepage Data with DMSContent integration"
    });
});

// Configure database settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // Allow all origins in development for testing
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
                               ?? new[] { "http://localhost:3000" };

            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Register services
builder.Services.AddScoped<IHomePageRepository, HomePageRepository>();
builder.Services.AddScoped<IHomePageService, HomePageService>();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ICHomePageData API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Add a simple root endpoint
app.MapGet("/", () => new
{
    Service = "ICHomePageData Microservice",
    Version = "1.0.0",
    Status = "Running",
    Timestamp = DateTime.UtcNow,
    Database = "DMSContent",
    Endpoints = new[]
    {
        "/api/homepage/statistics",
        "/api/homepage/carousel",
        "/api/homepage/key-stats",
        "/api/homepage/featured",
        "/api/homepage/recent-activities",
        "/api/homepage/health"
    }
});

// Configure to run on port 5216
app.Urls.Add("http://localhost:5216");

Console.WriteLine("ICHomePageData Microservice starting on http://localhost:5216");
Console.WriteLine("Using DMSContent database connection");
Console.WriteLine("Swagger UI available at http://localhost:5216");

app.Run();
