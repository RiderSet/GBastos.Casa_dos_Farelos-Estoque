namespace GBastos.Casa_dos_farelos_Estoque.Api.Dtos;

public sealed record ItemVendaDto(
    Guid ProdutoId,
    int Quantidade);