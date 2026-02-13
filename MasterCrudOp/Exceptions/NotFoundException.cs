using System.Net;

namespace MasterCrudOp.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resourceName,object key)
        : base($"{resourceName} with identifier '{key}' was not found.", HttpStatusCode.NotFound)
    {
    }
}
