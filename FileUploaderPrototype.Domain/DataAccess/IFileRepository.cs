using FileUploaderPrototype.Domain.Models;

namespace FileUploaderPrototype.Domain.DataAccess;

public interface IFileRepository
{
    /// <summary>
    /// Adds file to the Postgre database
    /// </summary>
    /// <param name="streamFile"></param>
    /// <returns></returns>
    Task AddAsync(StreamFile streamFile);
    
    /// <summary>
    /// Checks if file exists in storage based on name and modified date
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(BasicFileInfo file);
}