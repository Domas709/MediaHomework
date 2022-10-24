namespace FileUploaderPrototype.Domain.Models;

public class StreamFile
{
    public StreamFile(Stream stream, BasicFileInfo fileInfo)
    {
        Stream = stream;
        FileInfo = fileInfo;
    }
    
    public Stream Stream { get; }
    public BasicFileInfo FileInfo { get; }
}