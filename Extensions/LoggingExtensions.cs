using Serilog;
using Serilog.Enrichers;

namespace ProductCatalogApi.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}
