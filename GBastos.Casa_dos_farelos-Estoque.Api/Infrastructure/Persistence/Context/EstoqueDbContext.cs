using GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Persistence.Context;

public sealed class EstoqueDbContext : DbContext
{
    public DbSet<ProdutoEstoque> ProdutosEstoque => Set<ProdutoEstoque>();
    public DbSet<MovimentacaoEstoque> Movimentacoes => Set<MovimentacaoEstoque>();

    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> options)
        : base(options)
    {
    }

    public DbSet<EventoProcessado> EventosProcessados => Set<EventoProcessado>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ProdutoEstoque>(entity =>
        {
            entity.HasKey(x => x.ProdutoId);

            entity.Property(x => x.RowVersion)
                .IsRowVersion();
        });

        builder.Entity<MovimentacaoEstoque>(entity =>
        {
            entity.HasKey(x => x.Id);

            builder.Entity<EventoProcessado>().HasIndex(x => x.EventId).IsUnique();
        });
    }
}