using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AmountReceivedController : ControllerBase
    {
        private readonly ILogger<AmountReceivedController> _logger;
        private readonly IAmountReceivedRepository _amountReceivedRepository;

        public AmountReceivedController(ILogger<AmountReceivedController> logger,
            IAmountReceivedRepository amountReceivedRepository)
        {
            _logger = logger;
            _amountReceivedRepository = amountReceivedRepository;
        }

        /// <summary>
        /// Read all AmountReceived from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AmountReceived>> Get()
        {
            return await _amountReceivedRepository.GetAllAmountReceiveds();
        }

        /// <summary>
        /// Read AmountReceived from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAmountReceived")]
        public async Task<AmountReceived> GetRow(long amountReceivedId, int pageIndex, int pageSize)
        {
            return await _amountReceivedRepository.GetQuery(amountReceivedId, pageIndex, pageSize);
        }

        /// <summary>
        /// Read list of AmountReceived from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAmountReceivedWithPagging")]
        public async Task<List<AmountReceived>> GetamountReceivedWithPagging(int pageIndex, int pageSize)
        {
            var result = await _amountReceivedRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Create AmountReceived.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AmountReceived> Post(AmountReceived amountReceived)
        {
            return await _amountReceivedRepository.AddAmountReceivedAsync(amountReceived);
        }

        /// <summary>
        /// Update AmountReceived.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<AmountReceived> Put(AmountReceived amountReceived)
        {
            return await _amountReceivedRepository.UpdateAmountReceivedAsync(amountReceived);
        }

        /// <summary>
        /// Delete AmountReceived.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> Delete(long amountReceivedId, bool isHardDelete = false)
        {
            return await _amountReceivedRepository.DeleteAmountReceivedAsync(amountReceivedId, isHardDelete);
        }
    }
}
