using Arkano.Antifraud.Domain.Service;
using Arkano.Common.Common;
using Arkano.Common.Models;
using Moq;

namespace Arkano.Antifraud.Test.Domain
{
    [TestClass]
    public sealed class ProcessTransactionTest
    {
        private IAccumulated _accumulated;

        [TestInitialize]
        public void TestInitialize()
        {           
        }

        [TestMethod]
        public void ProcessTransaction_ValidTransaction_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(0);

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 100,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
             service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Approved, transaction.IdState);
        }

       

        [TestMethod]
        public void ProcessTransaction_InvalidTransaction_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(0);

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 2200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_InvalidTransactionAccumulate_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(20000);           

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_ValidTransaction_ChangeDate_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(20000);
            _accumulated.SetAccumulatedDate(DateTime.Today.AddDays(-1));            
            

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Approved, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_validTransaction_MultipleTransactionsAccumulated_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(0);

            var transaction1 = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 18000,
                IdState = (int)State.Pending
            };
            var transaction2 = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
            service.ValidateTransaction(ref transaction1);
            service.ValidateTransaction(ref transaction2);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction1.IdState);
            Assert.AreEqual((int)State.Approved, transaction2.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_InvalidTransaction_MultipleTransactionsAccumulated_ShouldProcess()
        {
            // Arrange
            _accumulated = new Accumulated();
            _accumulated.SetAccumulated(0);

            var transaction1 = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 18000,
                IdState = (int)State.Pending
            };
            var transaction2 = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 2100,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction(_accumulated);
            service.ValidateTransaction(ref transaction1);
            service.ValidateTransaction(ref transaction2);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction1.IdState);
            Assert.AreEqual((int)State.Rejected, transaction2.IdState);
        }
    }
}
