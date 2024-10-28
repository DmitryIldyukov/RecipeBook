namespace Application.Common.Result;

public class ResultT<T>
{
    public T Value { get; }
    public IReadOnlyList<string> ErrorMessages { get; }
    public bool IsSuccess => ErrorMessages.Count == 0;
    public string SuccessMessage { get; }

    private ResultT( T value, List<string> errorMessages, string successMessage )
    {
        Value = value;
        ErrorMessages = errorMessages ?? new List<string>();
        SuccessMessage = successMessage;
    }

    public static ResultT<T> Success( T value, string successMessage )
    {
        return new ResultT<T>( value, null, successMessage );
    }

    public static ResultT<T> Failure( IEnumerable<string> errorMessages )
    {
        return new ResultT<T>( default, new List<string>( errorMessages ), null );
    }

    public static ResultT<T> Failure( string errorMessage )
    {
        return new ResultT<T>( default, new List<string>() { errorMessage }, null );
    }
}
