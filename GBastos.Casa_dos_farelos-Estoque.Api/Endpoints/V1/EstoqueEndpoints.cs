using GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;
using GBastos.Casa_dos_farelos_Estoque.Api.Dtos;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Filters;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Endpoints.V1;

public static class EstoqueEndpoints
{
    public static RouteGroupBuilder MapEstoqueV1(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/estoque")
                       .WithTags("Estoque V1");

        group.MapGet("/{produtoId:guid}", GetEstoque);

        group.MapPost("/inicial", CargaInicial)
             .AddEndpointFilter<ValidationFilter>();

        return group;
    }

    private static async Task<IResult> GetEstoque(
        Guid produtoId,
        EstoqueDbContext db)
    {
        var estoque = await db.ProdutosEstoque.FindAsync(produtoId);

        return estoque is null
            ? Results.NotFound()
            : Results.Ok(estoque);
    }

    private static async Task<IResult> CargaInicial(
        List<ProdutoInicialDto> produtos,
        EstoqueDbContext db)
    {
        foreach (var p in produtos)
        {
            if (await db.ProdutosEstoque.AnyAsync(x => x.ProdutoId == p.ProdutoId))
                continue;

            db.ProdutosEstoque.Add(new ProdutoEstoque(p.ProdutoId, p.Quantidade));
        }

        await db.SaveChangesAsync();

        return Results.Ok();
    }
}