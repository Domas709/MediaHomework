using FileUploaderPrototype.Domain.Models;

namespace FileUploaderPrototype.Domain.DataAccess;

public interface ILocalStorageRepository
{
    /// <summary>
    /// Stores file to a configured local storage location
    /// </summary>
    /// <param name="streamFile"></param>
    /// <returns></returns>
    Task AddAsync(StreamFile streamFile);
}