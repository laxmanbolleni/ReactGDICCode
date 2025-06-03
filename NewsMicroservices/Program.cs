using Nest;
using NewsMicroservices.Data;
using NewsMicroservices.Models;
using NewsMicroservices.Repositories;
using NewsMicroservices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Elasticsearch settings
builder.Services.Configure<ElasticsearchSettings>(
    builder.Configuration.GetSection("ElasticsearchSettings"));

// Register Elasticsearch client
builder.Services.AddSingleton<ElasticsearchClientFactory>();
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var factory = provider.GetRequiredService<ElasticsearchClientFactory>();
    return factory.CreateClient();
});

// Register repositories and services
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
