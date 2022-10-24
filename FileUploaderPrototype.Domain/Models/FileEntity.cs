namespace FileUploaderPrototype;

public class FileEntity
{
    public string Name { get; set; }
    public byte[] Data { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}