using Arkano.Antifraud.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<IProcessTransaction, ProcessTransaction>();
            return services;
        }
    }
}
