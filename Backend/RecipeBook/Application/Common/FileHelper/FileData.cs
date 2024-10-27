namespace Application.Common.FileHelper;

public class FileData
{
    public FileStream File { get; init; }
    public string MimeType { get; init; }

    public FileData( FileStream file, string mimeType )
    {
        File = file;
        MimeType = mimeType;
    }
}
