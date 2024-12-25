using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using BaseClassLibrary.Repository;
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
        Task<List<PurchaseReports>> PurchaseReport();
        Task<List<StockReport>> StockReport();
        Task<long> GetMaxInvoiceNo();
    }
}

namespace Accounts.Core.Repositories
{
    public class PurchaseMasterRepository : IPurchaseMasterRepository
    {
        private readonly IBaseRepository<PurchaseMaster, AppDbContext> _purchaseMasterRepo;
        private readonly IBaseRepository<PurchaseReports, AppDbContext> _purchaseReportRepo;
        private readonly IBaseRepository<StockReport, AppDbContext> _stockReportRepo;


        public PurchaseMasterRepository(IBaseRepository<PurchaseMaster, AppDbContext> purchaseMasterRepo, IBaseRepository<PurchaseReports, AppDbContext> purchaseReportRepo, IBaseRepository<StockReport, AppDbContext> stockReportRepo)
        {
            _purchaseMasterRepo = purchaseMasterRepo;
            _purchaseReportRepo = purchaseReportRepo;
            _stockReportRepo = stockReportRepo;
        }

        public async Task<List<PurchaseReports>> PurchaseReport()
        {
            object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };
            ;
            var result = await _purchaseReportRepo.ExecuteStoredProcedureAsync("purchaseReport", paramerers);

            return result;
        }

        public async Task<long> GetMaxInvoiceNo()
        {
            try
            {
                var result = await _purchaseMasterRepo.QueryAsync(
                           query => query.Id > 0,
                           orderBy: c => c.CreatedDate ?? DateTime.Now,
                           0, 10);
                long invoiceNo = result.Max(x => x.InvoiceNo);

                return invoiceNo += 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PurchaseMaster> AddPurchaseMasterAsync(PurchaseMaster purchaseMaster)
        {
            try
            {
                long InvoiceNo = await GetMaxInvoiceNo();
                purchaseMaster.InvoiceNo = InvoiceNo;

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

        public async Task<List<PurchaseMaster>> GetAllPurchaseMasters(bool includeDetails = false)
        {
            Expression<Func<PurchaseMaster, bool>> predicate = c => c.Id > 0;
            if (includeDetails)
                return await _purchaseMasterRepo.GetAllAsync(predicate, x => x.PurchaseDetails);
            else
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

        public async Task<PurchaseMaster> UpdatePurchaseMasterAsync(PurchaseMaster purchaseMaster)
        {
            await _purchaseMasterRepo.UpdateAsync(purchaseMaster);
            return purchaseMaster;
        }

        public async Task<List<StockReport>> StockReport()
        {
            object[] paramerers = new object[] { };
            var result = await _stockReportRepo.ExecuteStoredProcedureAsync("stockReport", paramerers);

            return result;
        }
    }
}
