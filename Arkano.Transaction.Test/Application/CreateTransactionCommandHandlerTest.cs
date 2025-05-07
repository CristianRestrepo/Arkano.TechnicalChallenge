using Arkano.Common.Producer;
using Arkano.Transaction.Domain.Interfaces;
using Arkano.Transaction.Application.Transaction.Commands;
using AutoMapper;

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arkano.Common.Common;
using Arkano.Common.Models;
using Arkano.Transaction.Application.Transaction.Dto;
using Microsoft.Extensions.Logging;

namespace Arkano.Transaction.Test.Application
{
    [TestClass]
    public sealed class CreateTransactionCommandHandlerTest
    {
        private Mock<IDataContext> _dataContext;
        private IMapper _mapper;
        private Mock<IEventProducer> _producer;
        private Mock<ILogger<CreateTransactionCommandHandler>> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _dataContext = new Mock<IDataContext>();
            ;
            _producer = new Mock<IEventProducer>();
            _logger = new Mock<ILogger<CreateTransactionCommandHandler>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TransactionMappingProfile());
            });

            _mapper = new Mapper(config);
        }

        [TestMethod]
        public async Task Handle_ShouldCreateTransactionAndSendEvent()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                Value = 100
            };
            var transaction = new Domain.Entities.Transaction
            {
                Id = Guid.NewGuid(),
                SourceAccountId = command.SourceAccountId,
                TargetAccountId = command.TargetAccountId,
                TransferTypeId = command.TransferTypeId,
                IdState = (int)State.Pending,
                CreatedAd = DateTime.UtcNow,
                Value = command.Value
            };

            _dataContext.Setup(x => x.Transactions.Add(It.IsAny<Domain.Entities.Transaction>())).Callback<Domain.Entities.Transaction>(t => transaction.Id = t.Id);
            _dataContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);



            var handler = new CreateTransactionCommandHandler(_dataContext.Object, _mapper, _producer.Object, _logger.Object);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(transaction.Id, result.Id);
            _dataContext.Verify(x => x.Transactions.Add(It.IsAny<Domain.Entities.Transaction>()), Times.Once);
            _dataContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _producer.Verify(x => x.SendAsync<TransactionEvent>("Transactions", It.IsAny<TransactionEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldThrowException_WhenTransactionIsNull()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                SourceAccountId = Guid.NewGuid(),
                TargetAccountId = Guid.NewGuid(),
                TransferTypeId = 1,
                Value = 100
            };
            _dataContext.Setup(x => x.Transactions.Add(It.IsAny<Domain.Entities.Transaction>())).Throws(new Exception("Database error"));

            var handler = new CreateTransactionCommandHandler(_dataContext.Object, _mapper, _producer.Object, _logger.Object);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
    
    class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<Domain.Entities.Transaction, TransactionDto>()
            .ReverseMap();

            CreateMap<Domain.Entities.Transaction, TransactionEvent>().ReverseMap();
        }
    }
}
