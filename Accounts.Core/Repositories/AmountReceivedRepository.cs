using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IAmountReceivedRepository
    {
        Task<List<AmountReceived>> GetAllAmountReceiveds();
        Task<AmountReceived> AddAmountReceivedAsync(AmountReceived amountReceived);
        Task<AmountReceived> UpdateAmountReceivedAsync(AmountReceived amountReceived);
        Task<bool> DeleteAmountReceivedAsync(long amountReceivedId);
        Task<List<AmountReceived>> GetQuery(int pageIndex, int pageSize);
        Task<AmountReceived> GetQuery(long amountReceivedId, int pageIndex, int pageSize);
    }
}

namespace Accounts.Core.Repositories
{
    public class AmountReceivedRepository : IAmountReceivedRepository
    {
        private readonly IBaseRepository<AmountReceived, AppDbContext> _amountReceivedRepo;

        public AmountReceivedRepository(IBaseRepository<AmountReceived, AppDbContext> amountReceivedRepo)
        {
            _amountReceivedRepo = amountReceivedRepo;
        }

        public async Task<AmountReceived> AddAmountReceivedAsync(AmountReceived amountReceived)
        {
            try
            {
                await _amountReceivedRepo.BeginTransactionAsync();

                var result = await _amountReceivedRepo.AddAsync(amountReceived);

                await _amountReceivedRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _amountReceivedRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAmountReceivedAsync(long amountReceivedId)
        {
            await _amountReceivedRepo.DeleteAsync(amountReceivedId);
            return true;
        }

        public async Task<List<AmountReceived>> GetAllAmountReceiveds()
        {
            Expression<Func<AmountReceived, bool>> predicate = c => c.Id > 0;

            return await _amountReceivedRepo.GetAllAsync(predicate);
        }

        public async Task<List<AmountReceived>> GetQuery(int pageIndex, int pageSize)
        {
            return await _amountReceivedRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<AmountReceived> GetQuery(long amountReceivedId, int pageIndex, int pageSize)
        {
            var result = await _amountReceivedRepo.QueryAsync(
               query => query.Id == amountReceivedId,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<AmountReceived> UpdateAmountReceivedAsync(AmountReceived amountReceived)
        {
            await _amountReceivedRepo.UpdateAsync(amountReceived);
            return amountReceived;
        }
    }
}
