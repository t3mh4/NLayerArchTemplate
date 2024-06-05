using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace NLayerArchTemplate.WebUI.Helpers;

public class SeriLogHelper
{
    public static void Initialize(string connectionString)
    {
        //todo:aşağıdaki filtre kaldırıldığında beklenmedik hatalar için
        //aynı hataı 2 kez. yazmakata.Filtre aktif edildiğinde de bu sefer custom loglar görünmemekte.
        //düzeltilesi gerekiyor!!(CustomLogLevel yapılabilir.)
        var columnOptions = new ColumnOptions
        {
            AdditionalColumns = new Collection<SqlColumn>() { UserFullNameColumn,RequestType, RequestPath },
        };
        columnOptions.Store.Remove(StandardColumn.MessageTemplate);
        columnOptions.Store.Remove(StandardColumn.Properties);
        Log.Logger = new LoggerConfiguration()
            .WriteTo.MSSqlServer(connectionString,
                                 sinkOptions: MsSqlServerSinkOptions,
                                 sinkOptionsSection: null,
                                 appConfiguration: null,
                                 restrictedToMinimumLevel: LogEventLevel.Error,
                                 formatProvider: null,
                                 columnOptions: columnOptions,
                                 columnOptionsSection: null,
                                 logEventFormatter: null)
            .Enrich.FromLogContext()
            //.Filter.ByExcluding(Matching.WithProperty("SourceContext"))
            .CreateBootstrapLogger();
    }

    private static SqlColumn UserFullNameColumn
    {
        get
        {
            var col = new SqlColumn("UserFullName", SqlDbType.NVarChar, true, 150);
            col.PropertyName = "UserFullName";
            return col;
        }
    }

    private static SqlColumn RequestPath
    {
        get
        {
            var col = new SqlColumn("RequestPath", SqlDbType.NVarChar, true, 500);
            col.PropertyName = "RequestPath";
            return col;
        }
    }

    private static SqlColumn RequestType
    {
        get
        {
            var col = new SqlColumn("RequestType", SqlDbType.NVarChar, true, 10);
            col.PropertyName = "RequestType";
            return col;
        }
    }

    private static MSSqlServerSinkOptions MsSqlServerSinkOptions
    {
        get
        {
            return new MSSqlServerSinkOptions { TableName = "Log", AutoCreateSqlTable = true };
        }
    }
}
