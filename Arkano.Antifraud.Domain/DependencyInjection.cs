using Arkano.Antifraud.Domain.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Arkano.Antifraud.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IProcessTransaction, ProcessTransaction>();
            services.AddSingleton<IAccumulated, Accumulated>();
            return services;
        }
    }
}
