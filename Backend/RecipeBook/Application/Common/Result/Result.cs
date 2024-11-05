namespace Application.Common.Result;

public class Result
{
    public IReadOnlyList<string> ErrorMessages { get; }
    public string SuccessMessage { get; }
    public bool IsSuccess => ErrorMessages.Count == 0;

    private Result( List<string> errorMessages, string successMessage )
    {
        ErrorMessages = errorMessages ?? new List<string>();
        SuccessMessage = successMessage;
    }

    public static Result Success( string successMessage = null )
    {
        return new Result( null, successMessage );
    }

    public static Result Fail( IEnumerable<string> errorMessages )
    {
        return new Result( new List<string>( errorMessages ), null );
    }

    public static Result Fail( string errorMessage )
    {
        return new Result( new List<string> { errorMessage }, null );
    }
}
