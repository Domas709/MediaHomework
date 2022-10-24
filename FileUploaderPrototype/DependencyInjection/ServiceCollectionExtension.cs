using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models.Settings;
using FileUploaderPrototype.Services;
using FileUploaderPrototype.Services.Interfaces;

namespace FileUploaderPrototype.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static void AddDependencies(this IServiceCollection services)
    {
        AddOptions(services);

        services.AddApplicationInsightsTelemetryWorkerService();

        services.AddSingleton<IDataAccessService, DataAccessService>();
        services.AddSingleton<IFileManagerService, FileManagerService>();
    }

    private static void AddOptions(IServiceCollection services)
    {
        services
            .AddOptions<SftpSettings>()
            .BindConfiguration("SftpServer");
        services
            .AddOptions<SqlDbSettings>()
            .BindConfiguration("ConnectionStrings");
        services
            .AddOptions<LocalStorageSettings>()
            .BindConfiguration("LocalStorage");
    }
}