namespace GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;

public sealed class MovimentacaoEstoque
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public DateTime Data { get; private set; }

    private MovimentacaoEstoque() { }

    public MovimentacaoEstoque(Guid produtoId, int quantidade, string tipo)
    {
        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        Quantidade = quantidade;
        Tipo = tipo;
        Data = DateTime.UtcNow;
    }
}