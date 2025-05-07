namespace Arkano.Antifraud.Domain.Service
{
    public class Accumulated : IAccumulated
    {
        private int _accumulated = 0;
        private DateTime _accumulatedDate = DateTime.Today;

        public void AddValueToAccumulated(int value)
        {
            if (_accumulatedDate.Date.Equals(DateTime.Today.Date))
            {
               _accumulated += value;
            }
            else
            {
                _accumulatedDate = DateTime.Today;
                _accumulated = value;
            }
        }

        public int GetAccumulated()
        {
           return _accumulated;
        }

        public DateTime GetAccumulatedDate()
        {
            return _accumulatedDate;
        }


        public void SetAccumulated(int value)
        {
            _accumulated = value;
        }


        public void SetAccumulatedDate(DateTime date)
        {
            _accumulatedDate = date;
        }


    }
}
