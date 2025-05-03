using Arkano.Common.Producer;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Arkano.Antifraud.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IEventProducer, EventProducer>();            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
