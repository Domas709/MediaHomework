using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models;
using FileUploaderPrototype.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FileUploaderPrototype.Services;

public class FileManagerService : IFileManagerService
{
    private const int RetryAttempts = 3;
    
    private readonly IDataAccessService _dataAccessService;
    private readonly ILogger<FileManagerService> _logger;

    public FileManagerService(IDataAccessService dataAccessService, ILogger<FileManagerService> logger)
    {
        _dataAccessService = dataAccessService;
        _logger = logger;
    }

    public async Task ProcessNewFilesAsync()
    {
        var sftpRepository = _dataAccessService.GetSftpRepository();
        var fileRepository = _dataAccessService.GetFileRepository();
        var localStorageRepository = _dataAccessService.GetLocalStorageRepository();

        var allFilesBasicInfo = await sftpRepository.GetManyAsync();
        var newFilesBasicInfo = await GetNewFilesAsync(allFilesBasicInfo, fileRepository);

        if (newFilesBasicInfo.Count == 0)
        {
            _logger.LogInformation("No new files added to SFTP server. Ending process");
            return;
        }

        _logger.LogInformation($"Starting new file processing. New files count: {newFilesBasicInfo.Count}");
        var fileStreams = sftpRepository.GetManyFilesAsync(newFilesBasicInfo);

        await ProcessFileStreamsAsync(fileStreams, localStorageRepository, fileRepository, newFilesBasicInfo.Count);
    }

    private async Task ProcessFileStreamsAsync(IAsyncEnumerable<StreamFile> fileStreams, ILocalStorageRepository localStorageRepository,
        IFileRepository fileRepository, int totalNewFileCount)
    {
        var fileCount = 0;
        await foreach (var fileStream in fileStreams)
        {
            await using (fileStream.Stream)
            {
                var tries = 0;
                while (tries < RetryAttempts)
                {
                    try
                    {
                        await localStorageRepository.AddAsync(fileStream);
                        await fileRepository.AddAsync(fileStream);

                        fileCount++;
                        _logger.LogInformation($"Processed file {fileCount}/{totalNewFileCount}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        tries++;
                        _logger.LogError($"Error while adding {fileStream.FileInfo.Name} to local storage or Sql storage. Retry attempt {tries}: {ex}");
                        await Task.Delay(5000);
                    }
                }
            }
        }
    }

    private static async Task<List<BasicFileInfo>> GetNewFilesAsync(IEnumerable<BasicFileInfo> files, IFileRepository fileRepository)
    {
        var newFiles = new List<BasicFileInfo>();
        foreach (var file in files)
        {
            if (await fileRepository.ExistsAsync(file))
            {
                newFiles.Add(file);
            }
        }

        return newFiles;
    }
}