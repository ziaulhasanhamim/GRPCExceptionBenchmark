namespace GRPCExceptionBenchmark.Server;

public class ValidationException : Exception
{
    public ValidationException(string validationMessage)
        : base(validationMessage)
    {
        
    }
}