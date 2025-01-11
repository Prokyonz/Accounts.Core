using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ISalesDetailsRepository
    {
        Task<List<SalesDetails>> GetAllSalesDetails();
        Task<SalesDetails> AddSalesDetailsAsync(SalesDetails salesDetails);
        Task<SalesDetails> UpdateSalesDetailsAsync(SalesDetails salesDetails);
        Task<bool> DeleteSalesDetailsAsync(long salesDetailsId);
        Task<List<SalesDetails>> GetQuery(int pageIndex, int pageSize);
        Task<SalesDetails> GetQuery(long stockDetailsId, int pageIndex, int pageSize);
    }

    public class SalesDetailsRepository : ISalesDetailsRepository
    {
        private readonly IBaseRepository<SalesDetails, AppDbContext> _salesDetailsRepo;

        public SalesDetailsRepository(IBaseRepository<SalesDetails, AppDbContext> salesDetailsRepo)
        {
            _salesDetailsRepo = salesDetailsRepo;
        }

        public async Task<SalesDetails> AddSalesDetailsAsync(SalesDetails salesDetails)
        {
            try
            {
                await _salesDetailsRepo.BeginTransactionAsync();

                var result = await _salesDetailsRepo.AddAsync(salesDetails);

                await _salesDetailsRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _salesDetailsRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSalesDetailsAsync(long salesDetailsId)
        {
            await _salesDetailsRepo.DeleteAsync(salesDetailsId);
            return true;
        }

        public async Task<List<SalesDetails>> GetAllSalesDetails()
        {
            Expression<Func<SalesDetails, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;

            return await _salesDetailsRepo.GetAllAsync(predicate);
        }

        public async Task<List<SalesDetails>> GetQuery(int pageIndex, int pageSize)
        {
            return await _salesDetailsRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<SalesDetails> GetQuery(long stockDetailsId, int pageIndex, int pageSize)
        {
            var result = await _salesDetailsRepo.QueryAsync(
               query => query.Id == stockDetailsId && query.IsDelete == false,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<SalesDetails> UpdateSalesDetailsAsync(SalesDetails salesDetails)
        {
            await _salesDetailsRepo.UpdateAsync(salesDetails);
            return salesDetails;
        }
    }
}
