namespace GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;

public sealed class ProdutoEstoque
{
    public Guid ProdutoId { get; private set; }
    public int QuantidadeDisponivel { get; private set; }
    public byte[] RowVersion { get; private set; } = default!;

    private ProdutoEstoque() { }

    public ProdutoEstoque(Guid produtoId, int quantidadeInicial)
    {
        ProdutoId = produtoId;
        QuantidadeDisponivel = quantidadeInicial;
    }

    public void Debitar(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        if (QuantidadeDisponivel < quantidade)
            throw new InvalidOperationException("Estoque insuficiente.");

        QuantidadeDisponivel -= quantidade;
    }

    public void Creditar(int quantidade)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        QuantidadeDisponivel += quantidade;
    }
}