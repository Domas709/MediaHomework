using FileUploaderPrototype.Services.Interfaces;

namespace FileUploaderPrototype;

public class Worker : BackgroundService
{
    private readonly IFileManagerService _fileManagerService;
    private readonly ILogger<Worker> _logger;

    public Worker(IFileManagerService fileManagerService, ILogger<Worker> logger)
    {
        _fileManagerService = fileManagerService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                await _fileManagerService.ProcessNewFilesAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError($"New file processing failed due to the following exception:{exception}");
            }
            
            await Task.Delay(60000, stoppingToken);
        }
    }
}