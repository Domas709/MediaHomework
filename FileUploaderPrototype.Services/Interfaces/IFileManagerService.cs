namespace FileUploaderPrototype.Services.Interfaces;

public interface IFileManagerService
{
    /// <summary>
    /// Gets new files from SFTP storage and stores them at local location and Postgre database
    /// </summary>
    /// <returns></returns>
    Task ProcessNewFilesAsync();
}