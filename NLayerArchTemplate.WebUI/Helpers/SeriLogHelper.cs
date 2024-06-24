using Serilog;
using Serilog.Settings.Configuration;
using MySql.Data.MySqlClient;

namespace NLayerArchTemplate.WebUI.Helpers;

public class SeriLogHelper
{
    public static void Initialize(IConfiguration configuration, string connectionString)
    {
        var sectionName = "SerilogFile";
        if (IsDatabaseAvaliable(connectionString))
            sectionName = "SeriLogDb";
        Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration, new ConfigurationReaderOptions { SectionName = sectionName })
               .Enrich.FromLogContext()
               .CreateLogger();
    }

    private static bool IsDatabaseAvaliable(string connectionString)
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch (MySqlException ex)
        {
            return false;
        }
    }
}
   