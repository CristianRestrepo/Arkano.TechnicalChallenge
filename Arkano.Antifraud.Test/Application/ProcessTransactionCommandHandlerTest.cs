using Arkano.Antifraud.Domain.Services;
using Arkano.Antifraud.Application.Commands;
using Arkano.Common.Producer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Arkano.Common.Models;
using Arkano.Common.Common;

namespace Arkano.Antifraud.Test.Application
{
    [TestClass]
    public sealed class  ProcessTransactionCommandHandlerTest
    {
        private  Mock<IEventProducer> _producer;
        private  Mock<ILogger<ProcessTransactionCommandHandler>> _logger;
        private  Mock<IProcessTransaction> _processTransaction;

        [TestInitialize]
        public void TestInitialize()
        {
            _producer = new Mock<IEventProducer>();
            _logger = new Mock<ILogger<ProcessTransactionCommandHandler>>();
            _processTransaction = new Mock<IProcessTransaction>();
        }

        [TestMethod]
        public async Task Handle_ValidTransaction_ShouldProcessTransaction()
        {
            // Arrange
            var transactionEvent = new TransactionEvent
            {
                Id = Guid.NewGuid(),              
                Value = 100,
                IdState = (int)State.Pending
            };

            var command = new ProcessTransactionCommand
            {
                Event = transactionEvent                
            };

            var handler = new ProcessTransactionCommandHandler(_producer.Object, _logger.Object, _processTransaction.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            _producer.Verify(p => p.SendAsync<TransactionEvent>(It.IsAny<string>(), It.IsAny<TransactionEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_InvalidTransaction_ShouldThrowException()
        {
            // Arrange
            _producer.Setup(p => p.SendAsync<TransactionEvent>(It.IsAny<string>(), It.IsAny<TransactionEvent>()))
                .ThrowsAsync(new Exception("Error sending transaction"));

            var transactionEvent = new TransactionEvent
            {
                Id = Guid.NewGuid(),
                Value = 200,
                IdState = (int)State.Pending
            };
            var command = new ProcessTransactionCommand
            {
                Event = transactionEvent
            };
            var handler = new ProcessTransactionCommandHandler(_producer.Object, _logger.Object, _processTransaction.Object);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
