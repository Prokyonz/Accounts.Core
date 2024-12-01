using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IPurchaseMasterRepository
    {
        Task<List<PurchaseMaster>> GetAllPurchaseMasters(bool includeDetails);
        Task<PurchaseMaster> AddPurchaseMasterAsync(PurchaseMaster purchaseMaster);
        Task<PurchaseMaster> UpdatePurchaseMasterAsync(PurchaseMaster purchaseMaster);
        Task<bool> DeletePurchaseMasterAsync(long purchaseMasterId);
        Task<List<PurchaseMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails);
        Task<PurchaseMaster> GetQuery(long purchaseMasterId, int pageIndex, int pageSize, bool includeDetails);
        Task<List<PurchaseMaster>> PurchaseReport();
    }
}

namespace Accounts.Core.Repositories
{
    public class PurchaseMasterRepository : IPurchaseMasterRepository
    {
        private readonly IBaseRepository<PurchaseMaster, AppDbContext> _purchaseMasterRepo;

        public PurchaseMasterRepository(IBaseRepository<PurchaseMaster, AppDbContext> purchaseMasterRepo)
        {
            _purchaseMasterRepo = purchaseMasterRepo;
        }

        public async Task<PurchaseMaster> AddPurchaseMasterAsync(PurchaseMaster purchaseMaster)
        {
            try
            {
                await _purchaseMasterRepo.BeginTransactionAsync();

                var result = await _purchaseMasterRepo.AddAsync(purchaseMaster);

                await _purchaseMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _purchaseMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeletePurchaseMasterAsync(long purchaseMasterId)
        {
            await _purchaseMasterRepo.DeleteAsync(purchaseMasterId);
            return true;
        }

        public async Task<List<PurchaseMaster>> GetAllPurchaseMasters(bool includeDetails)
        {
            Expression<Func<PurchaseMaster, bool>> predicate = c => c.Id > 0;

            return await _purchaseMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<PurchaseMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _purchaseMasterRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<PurchaseMaster> GetQuery(long purchaseMasterId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            var result = await _purchaseMasterRepo.QueryAsync(
               query => query.Id == purchaseMasterId,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<List<PurchaseMaster>> PurchaseReport()
        {
            object[] paramerers = new object[] { "Id",1, "Name", "Abhishek" };

            var result = await _purchaseMasterRepo.ExecuteStoredProcedureAsync("purchaseReport", paramerers);
            
            return result;
        }

        public async Task<PurchaseMaster> UpdatePurchaseMasterAsync(PurchaseMaster purchaseMaster)
        {
            await _purchaseMasterRepo.UpdateAsync(purchaseMaster);
            return purchaseMaster;
        }
    }
}
