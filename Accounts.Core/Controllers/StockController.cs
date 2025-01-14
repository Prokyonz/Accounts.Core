using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IStockRepository _stockRepository;

        public StockController(ILogger<StockController> logger,
            IStockRepository stockRepository)
        {
            _logger = logger;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<List<Stock>> Get()
        {
            var result = await _stockRepository.GetAllStock();
            return result;
        }

        [HttpGet("GetStock")]
        public async Task<Stock> GetRow(long stockId, int pageIndex, int pageSize)
        {
            var result = await _stockRepository.GetQuery(stockId, pageIndex, pageSize);
            return result;
        }

        [HttpGet("GetStocksWithPagging")]
        public async Task<List<Stock>> GetStocksWithPagging(int pageIndex, int pageSize)
        {
            var result = await _stockRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<Stock> Post(Stock stock)
        {
            try
            {
                return await _stockRepository.AddStockAsync(stock);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<Stock> Put(Stock stock)
        {
            try
            {
                return await _stockRepository.UpdateStockAsync(stock);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(long stockId, bool isHardDelete = false)
        {
            try
            {
                return await _stockRepository.DeleteStockAsync(stockId, isHardDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
