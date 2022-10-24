using FileUploaderPrototype.DataAccess.Repositories;
using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileUploaderPrototype.Services;

public class DataAccessService : IDataAccessService
{
    private readonly SftpSettings _sftpSettings;
    private readonly SqlDbSettings _sqlDbSettings;
    private readonly LocalStorageSettings _localStorageSettings;
    private readonly ILoggerFactory _loggerFactory;

    public DataAccessService(
        IOptions<SftpSettings> sftpSettings,
        IOptions<SqlDbSettings> sqlDbSettings,
        IOptions<LocalStorageSettings> localStorageSettings,
        ILoggerFactory loggerFactory)
    {
        _sftpSettings = sftpSettings.Value;
        _sqlDbSettings = sqlDbSettings.Value;
        _localStorageSettings = localStorageSettings.Value;
        _loggerFactory = loggerFactory;
    }
    
    public IFileRepository GetFileRepository()
    {
        return new FileRepository(_sqlDbSettings);
    }

    public ISftpRepository GetSftpRepository()
    {
        return new SftpRepository(_sftpSettings, _loggerFactory.CreateLogger<SftpRepository>());
    }

    public ILocalStorageRepository GetLocalStorageRepository()
    {
        return new LocalStorageRepository(_localStorageSettings, _loggerFactory.CreateLogger<LocalStorageRepository>());
    }
}