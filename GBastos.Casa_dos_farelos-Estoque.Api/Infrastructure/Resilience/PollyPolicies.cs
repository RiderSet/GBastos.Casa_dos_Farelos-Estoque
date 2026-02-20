using Polly;
using Polly.Retry;

namespace GBastos.Casa_dos_farelos_Estoque.Api.Infrastructure.Resilience;

public static class PollyPolicies
{
    public static AsyncRetryPolicy RetryPolicy =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
}