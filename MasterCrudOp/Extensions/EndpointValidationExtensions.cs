using MasterCrudOp.Filters;

namespace MasterCrudOp.Extensions;

public static class EndpointValidationExtensions
{
    public static RouteHandlerBuilder AddValidation<T>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
    
}
