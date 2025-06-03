using DealsMicroservices.Models;
using DealsMicroservices.Data;
using DealsMicroservices.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add OpenAPI/Swagger
builder.Services.AddOpenApi();

// Configure Elasticsearch settings
builder.Services.Configure<ElasticsearchSettings>(
    builder.Configuration.GetSection("Elasticsearch"));

// Register services
builder.Services.AddSingleton<ElasticsearchClientFactory>();
builder.Services.AddScoped<IDealsRepository, DealsRepository>();
builder.Services.AddScoped<DealsMicroservices.Services.IDealsService, DealsMicroservices.Services.DealsService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors();

// Map controllers
app.MapControllers();

// Health check endpoint at root
app.MapGet("/", () => new
{
    service = "DealsMicroservices",
    status = "running",
    timestamp = DateTime.UtcNow
});

app.Run();
