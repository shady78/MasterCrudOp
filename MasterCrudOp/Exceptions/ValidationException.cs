using System.Net;

namespace MasterCrudOp.Exceptions;

public sealed class ValidationException : AppException
{
    public IDictionary<string , string[]> Errors { get; }
    public ValidationException(IDictionary<string , string[]> errors)
        : base("One or more validation errors Occured.",HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }
}
