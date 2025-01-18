using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using BaseClassLibrary.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IPurchaseMasterRepository
    {
        Task<List<PurchaseMaster>> GetAllPurchaseMasters(bool includeDetails);
        Task<PurchaseMaster> AddPurchaseMasterAsync(PurchaseMaster purchaseMaster);
        Task<PurchaseMaster> UpdatePurchaseMasterAsync(PurchaseMaster purchaseMaster);
        Task<bool> DeletePurchaseMasterAsync(long purchaseMasterId, bool isHardDelete = false);
        Task<List<PurchaseMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails);
        Task<PurchaseMaster> GetQuery(long purchaseMasterId, int pageIndex, int pageSize, bool includeDetails);
        Task<List<PurchaseReports>> PurchaseReport(long userId, DateTime? fromDate, DateTime? toDate, string? name);
        Task<List<StockReport>> StockReport(string salesId);
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
        private readonly AppDbContext _appDbContext;

        public PurchaseMasterRepository(IBaseRepository<PurchaseMaster, AppDbContext> purchaseMasterRepo, 
            IBaseRepository<PurchaseReports, AppDbContext> purchaseReportRepo, 
            IBaseRepository<StockReport, AppDbContext> stockReportRepo,
            AppDbContext appDbContext)
        {
            _purchaseMasterRepo = purchaseMasterRepo;
            _purchaseReportRepo = purchaseReportRepo;
            _stockReportRepo = stockReportRepo;
            _appDbContext = appDbContext;
        }

        public async Task<List<PurchaseReports>> PurchaseReport(long userId, DateTime? fromDate, DateTime? toDate, string? name)
        {
            //object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            string spName = "purchaseReport";
            if (userId > 0)
                spName += " " + userId;
            else
                spName += " " + "NULL";
            if (fromDate != null)
                spName += " ,'" + fromDate?.ToString("yyyyMMdd") + "'";
            else
                spName += " ," + "NULL";
            if (toDate != null)
                spName += " ,'" + toDate?.ToString("yyyyMMdd") + "'";
            else
                spName += " ," + "NULL";
            if (!string.IsNullOrWhiteSpace(name))
                spName += " ,'" + name + "'";
            else
                spName += " ," + "NULL";
            var result = await _purchaseReportRepo.ExecuteStoredProcedureAsync(spName);

            return result;
        }

        public async Task<long> GetMaxInvoiceNo()
        {
            try
            {
                var result = await _purchaseMasterRepo.QueryAsync(
                           query => query.Id > 0 && query.IsDelete == false,
                           orderBy: c => c.InvoiceNo,
                           0, int.MaxValue);
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

        public async Task<bool> DeletePurchaseMasterAsync(long purchaseMasterId, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                await _purchaseMasterRepo.DeleteAsync(purchaseMasterId);
            }
            else
            {
                await _purchaseMasterRepo.BeginTransactionAsync();
                var result = await _purchaseMasterRepo.GetByIdAsync(purchaseMasterId);
                result.UpdatedDate = DateTime.Now;
                result.IsDelete = true;
                await _purchaseMasterRepo.CommitTransactionAsync();
            }
            return true;
        }

        public async Task<List<PurchaseMaster>> GetAllPurchaseMasters(bool includeDetails = false)
        {
            Expression<Func<PurchaseMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;
            if (includeDetails)
                return await _purchaseMasterRepo.GetAllAsync(predicate, x => x.PurchaseDetails);
            else
                return await _purchaseMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<PurchaseMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _purchaseMasterRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<PurchaseMaster> GetQuery(long purchaseMasterId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            var result = await _purchaseMasterRepo.QueryAsync(
               query => query.Id == purchaseMasterId && query.IsDelete == false,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<PurchaseMaster> UpdatePurchaseMasterAsync(PurchaseMaster purchaseMaster)
        {
            await _purchaseMasterRepo.UpdateAsync(purchaseMaster);

            if (purchaseMaster.PurchaseDetails != null && purchaseMaster.PurchaseDetails.Any())
            {
                var purchaseDetails = await _appDbContext.PurchaseDetails.Where(x => x.PurchaseMasterId == purchaseMaster.Id).ToListAsync();

                if(purchaseDetails.Any())
                {
                    _appDbContext.PurchaseDetails.RemoveRange(purchaseDetails);
                }
            }

            return purchaseMaster;
        }

        public async Task<List<StockReport>> StockReport(string salesId)
        {
            var result = await _stockReportRepo.ExecuteStoredProcedureAsync("stockReport '"+ salesId +"'");

            return result;
        }
    }
}
