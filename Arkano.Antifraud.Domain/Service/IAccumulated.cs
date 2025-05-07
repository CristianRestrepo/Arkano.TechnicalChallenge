using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Service
{
    public interface IAccumulated
    {
        public int GetAccumulated();
        public void SetAccumulated(int value);
        public void AddValueToAccumulated(int value);
        public DateTime GetAccumulatedDate();
        public void SetAccumulatedDate(DateTime date);

    }
}
