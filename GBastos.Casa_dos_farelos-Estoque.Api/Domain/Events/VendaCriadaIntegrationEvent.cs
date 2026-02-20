using GBastos.Casa_dos_farelos_Estoque.Api.Dtos;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Domain.Events;

public sealed record VendaCriadaIntegrationEvent(
    Guid VendaId,
    List<ItemVendaDto> Itens);