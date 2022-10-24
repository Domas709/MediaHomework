using FileUploaderPrototype.Domain.DataAccess;
using FileUploaderPrototype.Domain.Models;
using FileUploaderPrototype.Domain.Models.Settings;

namespace FileUploaderPrototype.DataAccess.Repositories;

public class FileRepository : IFileRepository
{
    private readonly SqlDbSettings _settings;

    public FileRepository(SqlDbSettings settings)
    {
        _settings = settings;
    }
    
    public async Task AddAsync(StreamFile streamFile)
    {
        byte[] bytes;
        using (var binaryRead = new BinaryReader(streamFile.Stream))
        {
            bytes = binaryRead.ReadBytes((int)streamFile.Stream.Length);
        }
        
        using (var entities = new FileContext(_settings.SqlStorage))
        {
            entities.FileEntity.Add(new FileEntity
            {
                Name = streamFile.FileInfo.Name,
                Data = bytes,
                InsertDate = DateTime.UtcNow,
                ModifiedDate = streamFile.FileInfo.LastModifiedDate
            });
            await entities.SaveChangesAsync();
        }
    }

    public Task<bool> ExistsAsync(BasicFileInfo file)
    {
        using var entities = new FileContext(_settings.SqlStorage);
        return Task.FromResult(entities.FileEntity.Any(x => x.Name == file.Name && x.ModifiedDate == file.LastModifiedDate));
    }
}