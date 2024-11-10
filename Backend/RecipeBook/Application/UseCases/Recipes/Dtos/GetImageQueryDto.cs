namespace Application.UseCases.Recipes.Dtos;

public class GetImageQueryDto
{
    public FileStream File { get; init; }
    public string MimeType { get; init; }
    public string FileName { get; init; }

    public GetImageQueryDto( FileStream file, string mimeType, string fileName )
    {
        File = file;
        MimeType = mimeType;
        FileName = fileName;
    }
}
