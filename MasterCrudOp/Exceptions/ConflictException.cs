using System.Net;

namespace MasterCrudOp.Exceptions;

public class ConflictException : AppException
{
    public ConflictException(string message) 
        : base(message, HttpStatusCode.Conflict)
    {
    }
}
