using Arkano.Transaction.Application.Transaction.Commands;
using Arkano.Transaction.Application.Transaction.Dto;
using Arkano.Transaction.Application.Transaction.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Arkano.Transaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }



        /// <summary>
        /// Get transaction by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactionByIdOrDate")]
        [ProducesResponseType(typeof(List<TransactionDto?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransaction([FromQuery] GetTransactionQuery Query)
        {
            var transaction = await _mediator.Send(Query);
            if (transaction == null || !transaction.Any())
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("CreateTransaction")]
        [ProducesResponseType(typeof(TransactionDto),StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTransaction([FromQuery] CreateTransactionCommand command)
        {
            var transaction = await _mediator.Send(command);
            return Ok(transaction);
        }
    }
}
