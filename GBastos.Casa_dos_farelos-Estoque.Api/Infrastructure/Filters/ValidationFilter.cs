namespace GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Filters;

public sealed class ValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        if (context.Arguments.Any(a => a is null))
            return Results.BadRequest("Dados inválidos.");

        return await next(context);
    }
}