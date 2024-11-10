namespace Application.Common.FileHelper;

public interface IFileHelper
{
    void Save( string path, string fileName, Stream file );
    FileData Get( string filePath );
    void Delete( string fileName );
}
