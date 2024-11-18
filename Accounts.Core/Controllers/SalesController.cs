using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesMasterRepository _salesMasterRepository;

        public SalesController(ILogger<SalesController> logger,
            ISalesMasterRepository salesMasterRepository)
        {
            _logger = logger;
            _salesMasterRepository = salesMasterRepository;
        }

        [HttpGet]
        public async Task<List<SalesMaster>> Get()
        {
            var result = await _salesMasterRepository.GetAllSales();
            return result;
        }

        [HttpGet("GetSale")]
        public async Task<SalesMaster> GetRow(long salesId, int pageIndex, int pageSize)
        {
            var result = await _salesMasterRepository.GetQuery(salesId, pageIndex, pageSize);
            return result;
        }

        [HttpGet("GetSalesWithPagging")]
        public async Task<List<SalesMaster>> GetStocksWithPagging(int pageIndex, int pageSize)
        {
            var result = await _salesMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<SalesMaster> Post(SalesMaster salesMaster)
        {
            try
            {
                return await _salesMasterRepository.AddSalesAsync(salesMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<SalesMaster> Put(SalesMaster salesMaster)
        {
            try
            {
                return await _salesMasterRepository.UpdateSalesAsync(salesMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(long salesId)
        {
            try
            {
                return await _salesMasterRepository.DeleteSalesAsync(salesId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
