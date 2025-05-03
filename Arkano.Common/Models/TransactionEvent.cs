namespace Arkano.Common.Models
{
    public class TransactionEvent
    {
        public Guid Id { get; set; }      
        public int Value { get; set; }
        public int IdState { get; set; }
    }
}
