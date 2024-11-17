using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStock();
        Task<Stock> AddStockAsync(Stock customer);
        Task<Stock> UpdateStockAsync(Stock stock);
        Task<bool> DeleteStockAsync(long stockId);
        Task<List<Stock>> GetQuery(int pageIndex, int pageSize);
        Task<Stock> GetQuery(long stockId, int pageIndex, int pageSize);
    }

    public class StockRepository : IStockRepository
    {
        private readonly IBaseRepository<Stock, AppDbContext> _stockRepo;

        public StockRepository(IBaseRepository<Stock, AppDbContext> stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<Stock> AddStockAsync(Stock stock)
        {
            try
            {
                await _stockRepo.BeginTransactionAsync();

                var result = await _stockRepo.AddAsync(stock);

                await _stockRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _stockRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<Stock>> GetAllStock()
        {
            Expression<Func<Stock, bool>> predicate = c => c.Id > 0;

            return await _stockRepo.GetAllAsync(predicate);
        }

        public async Task<List<Stock>> GetQuery(int pageIndex, int pageSize)
        {
            return await _stockRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.Name ,
                pageIndex, pageSize);
        }

        public async Task<Stock> GetQuery(long stockId, int pageIndex, int pageSize)
        {
            var result = await _stockRepo.QueryAsync(
                query => query.Id == stockId,
                orderBy: c => c.Name,
                pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<Stock> UpdateStockAsync(Stock stock)
        {
            await _stockRepo.UpdateAsync(stock);
            return stock;
        }

        public async Task<bool> DeleteStockAsync(long stockId)
        {
            await _stockRepo.DeleteAsync(stockId);
            return true;
        }
    }
}
