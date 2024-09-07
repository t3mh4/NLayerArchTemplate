using NLayerArchTemplate.Core.Enums;
using Serilog.Settings.Configuration;
using Serilog;

namespace NLayerArchTemplate.WebUI.Configuration.BuilderServices;

public static class SeriLogService
{
    public static void Create(IConfiguration configuration)
    {
        var sectionName = "SerilogFile";
        if (configuration.GetValue<string>("LogType") == nameof(LogTypeEnum.Database))
            sectionName = "SeriLogDb";
        Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration, new ConfigurationReaderOptions { SectionName = sectionName })
               .Enrich.FromLogContext()
               .CreateLogger();
    }
}
