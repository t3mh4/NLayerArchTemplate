using Serilog;
using Serilog.Settings.Configuration;
using NLayerArchTemplate.Core.Enums;

namespace NLayerArchTemplate.WebUI.Helpers;

public class SeriLogHelper
{
    public static void Initialize(IConfiguration configuration)
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
   