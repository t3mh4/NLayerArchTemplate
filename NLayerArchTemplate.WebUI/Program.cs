using NLayerArchTemplate.WebUI.BuilderServices;
using NLayerArchTemplate.WebUI.Configuration.Pipeline;
using NLayerArchTemplate.WebUI.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var jsonFile = string.Concat("appsettings", environment == "Production" ? "" : "." + environment, ".json");
builder.Configuration.AddJsonFile(jsonFile, true, true);

builder.Services.Create(builder.Configuration);

#region SerilogConfig
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
SeriLogHelper.Initialize(conn);
builder.Host.UseSerilog();
#endregion

var app = builder.Build();
app.CreatePipeline();
app.Run();
