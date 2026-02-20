using GBastos.Casa_dos_farelos_Estoque.Api.Application.Handlers;
using GBastos.Casa_dos_farelos_Estoque.Api.Domain.Events;
using GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Resilience;
using StackExchange.Redis;
using System.Text.Json;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Messaging;

public sealed class VendaCriadaConsumer : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ISubscriber _subscriber;

    public VendaCriadaConsumer(
        IServiceProvider provider,
        IConnectionMultiplexer redis)
    {
        _provider = provider;
        _subscriber = redis.GetSubscriber();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync(
            RedisChannel.Literal("venda-criada"),
            async (channel, message) =>
            {
                using var scope = _provider.CreateScope();
                var handler = scope.ServiceProvider
                    .GetRequiredService<VendaCriadaHandler>();

                var evento = JsonSerializer
                    .Deserialize<VendaCriadaIntegrationEvent>(message!);

                if (evento is null)
                    return;

                try
                {
                    await PollyPolicies.RetryPolicy.ExecuteAsync(async () =>
                    {
                        await handler.HandleAsync(evento, stoppingToken);
                    });
                }
                catch (Exception)
                {
                    await _subscriber.PublishAsync(
                        RedisChannel.Literal("venda-criada-dlq"),
                        message);
                }
            });
    }
}