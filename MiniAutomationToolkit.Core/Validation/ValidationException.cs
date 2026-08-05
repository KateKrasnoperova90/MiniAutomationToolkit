namespace MiniAutomationToolkit.Core.Validation;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}