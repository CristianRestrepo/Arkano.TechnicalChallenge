using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Services
{
    public static class Accumulated
    {
        public static int _accumulated { get; set; } = 0;
        public static DateTime _accumulatedDate { get; set; } = DateTime.Now;
    }
}
