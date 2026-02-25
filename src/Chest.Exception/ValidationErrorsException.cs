namespace Chest.Exception;

public class ValidationErrorsException : ChestException
{
    public List<string> ErrorMessages { get; }

    public ValidationErrorsException(List<string> errorMessages) 
        : base(string.Empty) 
    {
        ErrorMessages = errorMessages;
    }
}