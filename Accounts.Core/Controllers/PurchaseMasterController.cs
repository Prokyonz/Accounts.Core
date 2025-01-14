using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseMasterController : ControllerBase
    {
        private readonly ILogger<PurchaseMasterController> _logger;
        private readonly IPurchaseMasterRepository _purchaseMasterRepository;

        public PurchaseMasterController(ILogger<PurchaseMasterController> logger,
            IPurchaseMasterRepository purchaseMasterRepository)
        {
            _logger = logger;
            _purchaseMasterRepository = purchaseMasterRepository;
        }

        /// <summary>
        /// Read all PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("PurchaseReport")]
        public async Task<List<PurchaseReports>> GetPurchaseReport([FromQuery] long userId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? name)
        {
            return await _purchaseMasterRepository.PurchaseReport(userId, fromDate, toDate, name);
        }

        /// <summary>
        /// Read all PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PurchaseMaster>> Get(bool includeDetails)
        {
            return await _purchaseMasterRepository.GetAllPurchaseMasters(includeDetails);
        }

        /// <summary>
        /// Read PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPurchaseMaster")]
        public async Task<PurchaseMaster> GetRow(long purchaseMasterId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _purchaseMasterRepository.GetQuery(purchaseMasterId, pageIndex, pageSize, includeDetails);
        }

        /// <summary>
        /// Read list of PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPurchaseMasterWithPagging")]
        public async Task<List<PurchaseMaster>> GetpurchaseMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _purchaseMasterRepository.GetQuery(pageIndex, pageSize, false);
            return result;
        }

        /// <summary>
        /// Create PurchaseMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PurchaseMaster> Post(PurchaseMaster purchaseMaster)
        {
            return await _purchaseMasterRepository.AddPurchaseMasterAsync(purchaseMaster);
        }

        /// <summary>
        /// Update PurchaseMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<PurchaseMaster> Put(PurchaseMaster purchaseMaster)
        {
            return await _purchaseMasterRepository.UpdatePurchaseMasterAsync(purchaseMaster);
        }

        /// <summary>
        /// Delete PurchaseMaster.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> Delete(long purchaseMasterId, bool isHardDelete = false)
        {
            return await _purchaseMasterRepository.DeletePurchaseMasterAsync(purchaseMasterId, isHardDelete);
        }

        /// <summary>
        /// Get Stock Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("StockReport/{salesId}")]
        public async Task<List<StockReport>> GetStockReport(string salesId)
        {
            return await _purchaseMasterRepository.StockReport(salesId);
        }
    }
}
