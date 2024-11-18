namespace Application.Common.Result;

public class ResultT<T>
{
    public T Value { get; }
    public IReadOnlyList<string> ErrorMessages { get; }
    public string SuccessMessage { get; }
    public bool IsSuccess => ErrorMessages.Count == 0;

    private ResultT( T value, List<string> errorMessages, string successMessage )
    {
        Value = value;
        ErrorMessages = errorMessages ?? new List<string>();
        SuccessMessage = successMessage;
    }

    public static ResultT<T> Success( T value, string successMessage = null )
    {
        return new ResultT<T>( value, null, successMessage );
    }

    public static ResultT<T> Fail( IEnumerable<string> errorMessages )
    {
        return new ResultT<T>( default, new List<string>( errorMessages ), null );
    }

    public static ResultT<T> Fail( string errorMessage )
    {
        return new ResultT<T>( default, new List<string>() { errorMessage }, null );
    }
}
