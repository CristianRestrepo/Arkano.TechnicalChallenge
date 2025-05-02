using Arkano.Common.Consumer;
using Arkano.Transaction.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Arkano.Transaction.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddScoped<IEventConsumer, EventConsumer>();            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddNpgsql<DataContext>(configuration.GetConnectionString("DefaultConnection"));
            services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());            
            return services;
        }
    }
}
