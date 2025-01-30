using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using Accounts.Core.Repositories;
using BaseClassLibrary.Interface;
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
        public async Task<List<SalesMaster>> Get(bool includeDetails = false)
        {
            var result = await _salesMasterRepository.GetAllSales(includeDetails);
            return result;
        }

        [HttpGet("GetSale/{salesId}")]
        public async Task<SalesMaster> GetRow(long salesId)
        {
            var result = await _salesMasterRepository.GetQuery(salesId, 0, 1, true);
            return result;
        }

        [HttpGet("GetSalesWithPagging")]
        public async Task<List<SalesMaster>> GetStocksWithPagging(int pageIndex, int pageSize, bool includeDetails = false)
        {
            var result = await _salesMasterRepository.GetQuery(pageIndex, pageSize, includeDetails);
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

        [HttpPut("updatepdf")]
        public async Task<bool> UpdatePDFOnly(long salesId, string pdf ="")
        {
            var result = await _salesMasterRepository.UpdatePDFOnly(salesId, pdf);
            return result;
        }

        [HttpDelete("{salesId}")]
        public async Task<bool> Delete(long salesId, bool isHardDelete = false)
        {
            try
            {
                return await _salesMasterRepository.DeleteSalesAsync(salesId, isHardDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Read all Sale from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SaleReport")]
        public async Task<List<SaleReport>> GetSaleReport([FromQuery] long userId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? name)
        {
            return await _salesMasterRepository.SalesReport(userId, fromDate, toDate, name);
        }

        /// <summary>
        /// Read all Sale from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SaleReportForAdmin")]
        public async Task<List<SaleReportForAdmin>> GetSaleReportForAdmin([FromQuery] long userId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? name)
        {
            return await _salesMasterRepository.SalesReportForAdmin(userId, fromDate, toDate, name);
        }

        /// <summary>
        /// Read all Sale from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SaleBillPrint/{saleMasterID}")]
        public async Task<SaleBillPrint> SaleBillPrint(long saleMasterID)
        {
            return await _salesMasterRepository.SalesBillPrint(saleMasterID);
        }
    }
}
