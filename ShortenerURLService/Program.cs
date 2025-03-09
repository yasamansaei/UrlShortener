using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using ShortenerURLService.Endpoints;
using ShortenerURLService.Infrastructures;
using ShortenerURLService.Infrastructures.Metrics;
using ShortenerURLService.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ShortenerUrlContext>(
    option =>
    {

        var database = builder.Configuration.GetSection("MongoDbConnections:DatabaseName").Value;
        var host = builder.Configuration.GetSection("MongoDbConnections:Host").Value;
        option.UseMongoDB(host, database);
    });


builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddPrometheusExporter();
        //metrics.AddAspNetCoreInstrumentation();
        //metrics.AddRuntimeInstrumentation();
        metrics.AddMeter(ShortenerUrlDiagnostics.MeterName);
    }
);

builder.Services.AddSingleton<ShortenerUrlDiagnostics>();
builder.Services.AddScoped<ShortenerUrlContext>();
builder.Services.AddTransient<ShortenerUrlService>();
var app = builder.Build();


app.UseStaticFiles();


app.MapShortnerUrl();
app.MapRedirectUrl();
//app.MapMetricUrl();


app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();

app.Run();

