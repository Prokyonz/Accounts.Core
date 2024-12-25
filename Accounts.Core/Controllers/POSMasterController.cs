using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class POSMasterController : ControllerBase
    {
        private readonly ILogger<POSMasterController> _logger;
        private readonly IPOSMasterRepository _pOSMasterRepository;

        public POSMasterController(ILogger<POSMasterController> logger,
            IPOSMasterRepository pOSMasterRepository)
        {
            _logger = logger;
            _pOSMasterRepository = pOSMasterRepository;
        }

        /// <summary>
        /// Read all POSMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<POSMaster>> Get()
        {
            return await _pOSMasterRepository.GetAllPOSMasters();
        }

        /// <summary>
        /// Read POSMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPOSMaster")]
        public async Task<POSMaster> GetRow(long pOSMasterId, int pageIndex, int pageSize)
        {
            return await _pOSMasterRepository.GetQuery(pOSMasterId, pageIndex, pageSize);
        }

        /// <summary>
        /// Read list of POSMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPOSMasterWithPagging")]
        public async Task<List<POSMaster>> GetpOSMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _pOSMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Create POSMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<POSMaster> Post(POSMaster pOSMaster)
        {
            return await _pOSMasterRepository.AddPOSMasterAsync(pOSMaster);
        }

        /// <summary>
        /// Update POSMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<POSMaster> Put(POSMaster pOSMaster)
        {
            return await _pOSMasterRepository.UpdatePOSMasterAsync(pOSMaster);
        }

        /// <summary>
        /// Delete POSMaster.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> Delete(long pOSMasterId)
        {
            return await _pOSMasterRepository.DeletePOSMasterAsync(pOSMasterId);
        }
    }
}
