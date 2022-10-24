using System.Data.Entity;

namespace FileUploaderPrototype.DataAccess;

public class FileContext : DbContext
{
    public FileContext()
        : base()
    {
    }

    public FileContext(string connectionString)
        : base(nameOrConnectionString: connectionString)
    {
    }

    public DbSet<FileEntity> FileEntity { get; set; }
}