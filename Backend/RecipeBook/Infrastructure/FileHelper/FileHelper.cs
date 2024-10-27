using Application.Common.FileHelper;
using MimeMapping;

namespace Infrastructure.FileHelper;

public class FileHelper : IFileHelper
{
    public void Delete( string fileName )
    {
        if ( File.Exists( fileName ) )
            File.Delete( fileName );
    }

    public FileData Get( string filePath )
    {
        if ( !File.Exists( filePath ) )
            throw new FileNotFoundException( "Файл не найден." );

        var contentType = MimeUtility.GetMimeMapping( filePath );

        FileData file = new FileData( File.OpenRead( filePath ), contentType );

        return file;
    }

    public void Save( string path, string fileName, Stream file )
    {
        if ( !Directory.Exists( path ) ) Directory.CreateDirectory( path );

        string fullPath = Path.Combine( path, fileName );

        using ( var fileOut = File.Create( fullPath ) )
        {
            file.Seek( 0, SeekOrigin.Begin );
            file.CopyTo( fileOut );
        }
    }
}