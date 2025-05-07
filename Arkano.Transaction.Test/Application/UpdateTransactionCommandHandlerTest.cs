using Arkano.Common.Common;
using Arkano.Transaction.Application.Transaction.Commands;
using Arkano.Transaction.Domain.Interfaces;
using Arkano.Transaction.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Transaction.Test.Application
{
    [TestClass]
    public sealed class UpdateTransactionCommandHandlerTest
    {
        private IDataContext _dataContext;
        private Mock<ILogger<UpdateTransactionCommandHandler>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {

            _logger = new Mock<ILogger<UpdateTransactionCommandHandler>>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataContext = new DataContext(options);

        }

        [TestMethod]
        public async Task Handle_ShouldUpdateTransactionState()
        {
            // Arrange
            var command = new UpdateTransactionCommand
            {
                Id = Guid.NewGuid(),
                IdState = (int)State.Approved
            };

            var transaction = new Domain.Entities.Transaction
            {
                Id = command.Id,
                IdState = (int)State.Pending,
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                CreatedAd = DateTime.UtcNow,
                Value = 100
            };

            _dataContext.Transactions.Add(transaction);
            await _dataContext.SaveChangesAsync();

            var handler = new UpdateTransactionCommandHandler(_dataContext, _logger.Object);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Handle_ShouldUpdateTransactionState_ThrowException()
        {
            // Arrange
            var command = new UpdateTransactionCommand
            {
                Id = Guid.NewGuid(),
                IdState = (int)State.Approved
            };

            var handler = new UpdateTransactionCommandHandler(_dataContext, _logger.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_Database_ThrowException()
        {
            // Arrange
            var command = new UpdateTransactionCommand
            {
                Id = Guid.NewGuid(),
                IdState = (int)State.Approved
            };

            Mock<IDataContext> _context = new Mock<IDataContext>();
            _context.Setup(x=>x.Transactions).Throws<Exception>();

            var handler = new UpdateTransactionCommandHandler(_dataContext, _logger.Object);
            await Assert.ThrowsExceptionAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
