namespace GBastos.Casa_dos_farelos_Estoque.Api.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealth(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health")
           .WithTags("Health");
    }
}