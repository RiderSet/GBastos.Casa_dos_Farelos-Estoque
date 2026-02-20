using GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;
using GBastos.Casa_dos_farelos_Estoque.Api.Domain.Events;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Application.Handlers;

public sealed class VendaCriadaHandler
{
    private readonly EstoqueDbContext _db;

    public VendaCriadaHandler(EstoqueDbContext db)
    {
        _db = db;
    }

    public async Task HandleAsync(VendaCriadaIntegrationEvent evento, CancellationToken ct)
    {
        if (await _db.EventosProcessados
            .AnyAsync(x => x.EventId == evento.VendaId.ToString(), ct))
            return;

        foreach (var item in evento.Itens)
        {
            var estoque = await _db.ProdutosEstoque.FindAsync([item.ProdutoId], ct);
            estoque!.Debitar(item.Quantidade);

            _db.Movimentacoes.Add(
                new MovimentacaoEstoque(item.ProdutoId, item.Quantidade, "Saida"));
        }

        _db.EventosProcessados.Add(
            new EventoProcessado(evento.VendaId.ToString()));

        await _db.SaveChangesAsync(ct);
    }
}