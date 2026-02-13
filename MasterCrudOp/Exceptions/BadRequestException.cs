using System.Net;

namespace MasterCrudOp.Exceptions;

public sealed class BadRequestException : AppException
{
    public BadRequestException(string message) 
        : base(message , HttpStatusCode.BadRequest)
    {
    }
}
