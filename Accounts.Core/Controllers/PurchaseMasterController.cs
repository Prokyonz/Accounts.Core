using Accounts.Core.Models;
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
        public async Task<List<PurchaseMaster>> GetPurchaseReport()
        {
            return await _purchaseMasterRepository.PurchaseReport();
        }

        /// <summary>
        /// Read all PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PurchaseMaster>> Get()
        {
            return await _purchaseMasterRepository.GetAllPurchaseMasters();
        }

        /// <summary>
        /// Read PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPurchaseMaster")]
        public async Task<PurchaseMaster> GetRow(long purchaseMasterId, int pageIndex, int pageSize)
        {
            return await _purchaseMasterRepository.GetQuery(purchaseMasterId, pageIndex, pageSize);
        }

        /// <summary>
        /// Read list of PurchaseMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPurchaseMasterWithPagging")]
        public async Task<List<PurchaseMaster>> GetpurchaseMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _purchaseMasterRepository.GetQuery(pageIndex, pageSize);
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
        public async Task<bool> Delete(long purchaseMasterId)
        {
            return await _purchaseMasterRepository.DeletePurchaseMasterAsync(purchaseMasterId);
        }
    }
}
