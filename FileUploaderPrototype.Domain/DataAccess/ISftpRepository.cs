using FileUploaderPrototype.Domain.Models;

namespace FileUploaderPrototype.Domain.DataAccess;

public interface ISftpRepository
{
    /// <summary>
    /// Gets all files basic information in configured folder
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<BasicFileInfo>> GetManyAsync();
    
    
    /// <summary>
    /// Gets a stream of streams of provided files
    /// </summary>
    /// <param name="fileNames"></param>
    /// <returns></returns>
    IAsyncEnumerable<StreamFile> GetManyFilesAsync(IEnumerable<BasicFileInfo> fileNames);
}