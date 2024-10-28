namespace Application.Common.Result;

public class Result
{
    public IReadOnlyList<string> ErrorMessages { get; }
    public bool IsSuccess => ErrorMessages.Count == 0;
    public string SuccessMessage { get; }

    private Result( List<string> errorMessages, string successMessage )
    {
        ErrorMessages = errorMessages ?? new List<string>();
        SuccessMessage = successMessage;
    }

    public static Result Success( string successMessage = null )
    {
        return new Result( null, successMessage );
    }

    public static Result Failure( IEnumerable<string> errorMessages )
    {
        return new Result( new List<string>( errorMessages ), null );
    }

    public static Result Failure( string errorMessage )
    {
        return new Result( new List<string> { errorMessage }, null );
    }
}
