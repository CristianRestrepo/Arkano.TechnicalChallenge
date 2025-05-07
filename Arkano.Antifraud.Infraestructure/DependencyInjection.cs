using Arkano.Antifraud.Infrastructure.Service;
using Arkano.Common.Consumer;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Arkano.Antifraud.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEventConsumer, EventConsumer>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));            
            return services;
        }
    }
}
