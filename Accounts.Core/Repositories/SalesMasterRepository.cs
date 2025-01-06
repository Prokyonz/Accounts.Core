using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ISalesMasterRepository
    {
        Task<List<SalesMaster>> GetAllSales(bool includeDetails);
        Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster);
        Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster);
        Task<bool> DeleteSalesAsync(long salesId);
        Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails);
        Task<SalesMaster> GetQuery(long stockId, int pageIndex, int pageSize, bool includeDetails);
        Task<List<SaleReport>> SalesReport(long userId, DateTime? fromDate, DateTime? toDate, string? name);
        Task<long> GetMaxInvoiceNo();
    }

    public class SalesMasterRepository : ISalesMasterRepository
    {
        private readonly IBaseRepository<SalesMaster, AppDbContext> _salesRepo;
        private readonly IBaseRepository<SaleReport, AppDbContext> _salesReportRepo;
        private readonly IBaseRepository<SeriesMaster, AppDbContext> _seriesMasterRepo;

        public SalesMasterRepository(IBaseRepository<SalesMaster, AppDbContext> salesRepo, 
            IBaseRepository<SaleReport, AppDbContext> salesReportRepo,
            IBaseRepository<SeriesMaster, AppDbContext> seriesMasterRepo)
        {
            _salesRepo = salesRepo;
            _salesReportRepo = salesReportRepo;
            _seriesMasterRepo = seriesMasterRepo;
        }

        public async Task<List<SaleReport>> SalesReport(long userId, DateTime? fromDate, DateTime? toDate, string? name)
        {
            //object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            string spName = "salesReport";
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
            var result = await _salesReportRepo.ExecuteStoredProcedureAsync(spName);

            return result;
        }

        public async Task<long> GetMaxInvoiceNo()
        {
            try
            {
                var result = await _salesRepo.QueryAsync(
                           query => query.Id > 0,
                           orderBy: c => c.InvoiceNo,
                           0, int.MaxValue);

                long invoiceNo = 0;
                if (result != null && result.Count > 0)
                {
                    invoiceNo = result.Max(x => x.InvoiceNo);
                }

                return invoiceNo += 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster)
        {
            try
            {
                var series = await _seriesMasterRepo.QueryAsync(
                           query => query.Id > 0,
                           orderBy: c => c.CreatedDate ?? DateTime.Now,
                           0, 10);

                salesMaster.SeriesName = series[0].Name;

                salesMaster.InvoiceNo = await GetMaxInvoiceNo();

                await _salesRepo.BeginTransactionAsync();

                var result = await _salesRepo.AddAsync(salesMaster);

                await _salesRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _salesRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteSalesAsync(long salesId)
        {
            await _salesRepo.DeleteAsync(salesId);
            return true;
        }

        public async Task<List<SalesMaster>> GetAllSales(bool includeDetails = false)
        {
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id > 0;
            Expression<Func<SalesMaster, object>> salesDetails = x => x.SalesDetails;
            Expression<Func<SalesMaster, object>> amountReceived = m => m.AmountReceived;

            if (includeDetails)
                return await _salesRepo.GetAllAsync(predicate, salesDetails, amountReceived);
            else
                return await _salesRepo.GetAllAsync(predicate);
        }

        public async Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _salesRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.EntryDate,
                pageIndex, pageSize);
        }

        public async Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            var result = await _salesRepo.QueryAsync(
               query => query.Id == salesId,
               orderBy: c => c.EntryDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster)
        {
            await _salesRepo.UpdateAsync(salesMaster);
            return salesMaster;
        }
    }
}
