namespace GBastos.Casa_dos_farelos_Estoque.Api.Domain.Entities;

public sealed class EventoProcessado
{
    public Guid Id { get; private set; }
    public string EventId { get; private set; } = string.Empty;
    public DateTime ProcessadoEm { get; private set; }

    private EventoProcessado() { }

    public EventoProcessado(string eventId)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        ProcessadoEm = DateTime.UtcNow;
    }
}