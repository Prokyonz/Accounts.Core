using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesDetailsController : ControllerBase
    {
        private readonly ILogger<SalesDetailsController> _logger;
        private readonly ISalesDetailsRepository _salesDetailsRepository;

        public SalesDetailsController(ILogger<SalesDetailsController> logger,
            ISalesDetailsRepository salesDetailsRepository)
        {
            _logger = logger;
            _salesDetailsRepository = salesDetailsRepository;
        }

        [HttpGet]
        public async Task<List<SalesDetails>> Get()
        {
            var result = await _salesDetailsRepository.GetAllSalesDetails();
            return result;
        }

        [HttpGet("GetSaleDetails")]
        public async Task<SalesDetails> GetRow(long salesDetailsId, int pageIndex, int pageSize)
        {
            var result = await _salesDetailsRepository.GetQuery(salesDetailsId, pageIndex, pageSize);
            return result;
        }

        [HttpGet("GetSalesDetailsWithPagging")]
        public async Task<List<SalesDetails>> GetStocksWithPagging(int pageIndex, int pageSize)
        {
            var result = await _salesDetailsRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<SalesDetails> Post(SalesDetails salesDetails)
        {
            try
            {
                return await _salesDetailsRepository.AddSalesDetailsAsync(salesDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<SalesDetails> Put(SalesDetails salesDetails)
        {
            try
            {
                return await _salesDetailsRepository.UpdateSalesDetailsAsync(salesDetails);
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
                return await _salesDetailsRepository.DeleteSalesDetailsAsync(salesId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
