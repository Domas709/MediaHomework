using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models;
using FileUploaderPrototype.Domain.Models.Settings;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace FileUploaderPrototype.DataAccess.Repositories;

public class SftpRepository : ISftpRepository
{
    private readonly SftpSettings _sftpSettings;
    private readonly ILogger<SftpRepository> _logger;

    public SftpRepository(SftpSettings sftpSettings, ILogger<SftpRepository> logger)
    {
        _sftpSettings = sftpSettings;
        _logger = logger;
    }
    
    public async Task<IEnumerable<BasicFileInfo>> GetManyAsync()
    {
        var client = GetSftpClient();
        var directoryList = await ListDirectoryAsync(client, ".");
        var basicFilesInfo = directoryList.Select(x => new BasicFileInfo
        {
            Name = x.Name,
            LastModifiedDate = x.LastWriteTimeUtc
        });

        return basicFilesInfo;
    }

    public async IAsyncEnumerable<StreamFile> GetManyFilesAsync(IEnumerable<BasicFileInfo> filesInfo)
    {
        var client = GetSftpClient();
        
        foreach (var fileInfo in filesInfo)
        {
            yield return new StreamFile(await DownloadFileAsync(client, fileInfo.Name), fileInfo);
        }
    }

    private SftpClient GetSftpClient()
    {
        using var client = new SftpClient(_sftpSettings.Host, _sftpSettings.Username, _sftpSettings.Password);
        client.Connect();
        return client;
    }

    private Task<IEnumerable<SftpFile>> ListDirectoryAsync(SftpClient client, string path)
    {
        var tcs = new TaskCompletionSource<IEnumerable<SftpFile>>();
        client.BeginListDirectory(path, asyncResult =>
        {
            try
            {
                tcs.TrySetResult(client.EndListDirectory(asyncResult));
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                _logger.LogError($"Error while listing files: {ex}");
            }
        }, null);
        
        return tcs.Task;
    }
    
    private Task<Stream> DownloadFileAsync(SftpClient client, string path)
    {
        var tcs = new TaskCompletionSource<Stream>();
        var str = new MemoryStream();
        client.BeginDownloadFile(path, str, asyncResult =>
        {
            try
            {
                tcs = new TaskCompletionSource<Stream>(str);
                client.EndDownloadFile(asyncResult);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
                _logger.LogError($"Error while downloading file: {ex}");
            }
        }, null);
        
        return tcs.Task;
    }
}