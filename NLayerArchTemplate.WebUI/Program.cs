using NLayerArchTemplate.WebUI.BuilderServices;
using NLayerArchTemplate.WebUI.Configuration.Pipeline;
using NLayerArchTemplate.WebUI.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
var environmentName = builder.Environment.EnvironmentName;
var jsonFile = string.Concat("appsettings", environmentName == Environments.Production ? "" : "." + environmentName, ".json");
builder.Configuration.AddJsonFile(jsonFile, true, true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Create(builder.Configuration, isDevelopment, connectionString);

#region SerilogConfig
SeriLogHelper.Initialize(builder.Configuration, connectionString);
builder.Host.UseSerilog();
#endregion

var app = builder.Build();
app.CreatePipeline();
app.Run();
