﻿using DAL.PostgresSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

    services
        .AddSingleton<ITemperatureRepository, TemperatureRepository>();
});

var app = builder.Build();

app.UseRouting();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();

public partial class Program { }
