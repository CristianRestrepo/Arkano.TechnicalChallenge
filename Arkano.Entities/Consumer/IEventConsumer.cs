using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arkano.Common.Consumer
{
    public interface IEventConsumer
    {
        Task Consume(string topic);
    }
}