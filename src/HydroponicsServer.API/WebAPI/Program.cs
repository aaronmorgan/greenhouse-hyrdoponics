using DAL.PostgresSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog(logger);

builder.Services.AddSingleton(logger);

builder.Host.ConfigureServices((context, services) =>
{
    services.AddControllers();
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    });

    services
      .AddMemoryCache()
      .AddMvcCore()
      .AddAuthorization()
      .AddNewtonsoftJson(options =>
      {
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      });

    //services
    //    .AddSingleton<ITemperatureRepository, TemperatureRepository>()
    //    .AddSingleton<IINA219Repository, INA219Repository>();
});

var app = builder.Build();

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseMetricServer();
app.UseHttpMetrics();
app.MapControllers();


app.Run();

public partial class Program { }
