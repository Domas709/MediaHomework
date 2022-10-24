using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models;
using FileUploaderPrototype.Domain.Models.Settings;
using Microsoft.Extensions.Logging;

namespace FileUploaderPrototype.DataAccess.Repositories;

public class LocalStorageRepository : ILocalStorageRepository
{
    private readonly LocalStorageSettings _localStorageSettings;
    private readonly ILogger<LocalStorageRepository> _logger;

    public LocalStorageRepository(LocalStorageSettings localStorageSettings, ILogger<LocalStorageRepository> logger)
    {
        _localStorageSettings = localStorageSettings;
        _logger = logger;
    }

    public async Task AddAsync(StreamFile streamFile)
    {
        await using Stream destinationStream = File.Create(_localStorageSettings.Path + streamFile.FileInfo.Name);
        await CopyStreamAsync(streamFile.Stream, destinationStream);
    }
    
    /// <summary>
    /// Copies the contents of input to output. Doesn't close either stream.
    /// </summary>
    private async Task CopyStreamAsync(Stream input, Stream output)
    {
        try
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = await input.ReadAsync(buffer)) > 0)
            {
                await output.WriteAsync(buffer.AsMemory(0, len));
            }    
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while copying stream to local storage: {ex}");
            throw;
        }
    }
}