namespace FileUploaderPrototype.Domain.DataAccess;

/// <summary>
/// Used for getting new instances of repositories
/// </summary>
public interface IDataAccessService
{
    IFileRepository GetFileRepository();
    ISftpRepository GetSftpRepository();
    ILocalStorageRepository GetLocalStorageRepository();
}