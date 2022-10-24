using FileUploaderPrototype;
using FileUploaderPrototype.DependencyInjection;
using Microsoft.ApplicationInsights;
using Serilog;
using Serilog.Events;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(builder => builder.AddEnvironmentVariables("ASPNETCORE_"))
    .UseSerilog((hostContext, serviceProvider, loggerConfiguration) =>
    {
        var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
        loggerConfiguration
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.ApplicationInsights(telemetryClient, TelemetryConverter.Traces, LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    })
    .ConfigureServices((services) =>
    {
        services.AddHostedService<Worker>();
        services.AddDependencies();
    })
    .Build();

await host.RunAsync();