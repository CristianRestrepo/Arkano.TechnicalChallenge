using Arkano.Antifraud.Domain.Services;
using Arkano.Common.Common;
using Arkano.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Test.Domain
{
    [TestClass]
    public sealed class ProcessTransactionTest
    {
        [TestMethod]
        public void ProcessTransaction_ValidTransaction_ShouldProcess()
        {
            // Arrange
            Accumulated._accumulated = 0;

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 100,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction();
             service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Approved, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_InvalidTransaction_ShouldProcess()
        {
            // Arrange
            Accumulated._accumulated = 0;

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 2200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction();
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_InvalidTransactionAccumulate_ShouldProcess()
        {
            // Arrange
            Accumulated._accumulated = 20000;

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction();
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_ValidTransaction_ChangeDate_ShouldProcess()
        {
            // Arrange
            Accumulated._accumulated = 20000;
            Accumulated._accumulatedDate = DateTime.Today.AddDays(-1);

            var transaction = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };

            // Act
            var service = new ProcessTransaction();
            service.ValidateTransaction(ref transaction);

            // Assert
            Assert.AreEqual((int)State.Approved, transaction.IdState);
        }

        [TestMethod]
        public void ProcessTransaction_validTransaction_MultipleTransactionsAccumulated_ShouldProcess()
        {
            // Arrange
            Accumulated._accumulated = 0;

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
            var service = new ProcessTransaction();
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
            Accumulated._accumulated = 0;

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
            var service = new ProcessTransaction();
            service.ValidateTransaction(ref transaction1);
            service.ValidateTransaction(ref transaction2);

            // Assert
            Assert.AreEqual((int)State.Rejected, transaction1.IdState);
            Assert.AreEqual((int)State.Rejected, transaction2.IdState);
        }
    }
}
