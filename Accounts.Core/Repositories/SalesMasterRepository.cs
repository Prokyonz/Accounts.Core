using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Accounts.Core.Repositories
{
    public interface ISalesMasterRepository
    {
        Task<List<SalesMaster>> GetAllSales(bool includeDetails);
        Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster);
        Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster);
        Task<bool> DeleteSalesAsync(long salesId);
        Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails);
        Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize, bool includeDetails);
        Task<List<SaleReport>> SalesReport(long userId, DateTime? fromDate, DateTime? toDate, string? name);
        Task<long> GetMaxInvoiceNo();
    }

    public class SalesMasterRepository : ISalesMasterRepository
    {
        private readonly IBaseRepository<SalesMaster, AppDbContext> _salesRepo;
        private readonly IBaseRepository<SaleReport, AppDbContext> _salesReportRepo;
        private readonly IBaseRepository<SeriesMaster, AppDbContext> _seriesMasterRepo;
        private readonly AppDbContext _appDbContext;

        public SalesMasterRepository(IBaseRepository<SalesMaster, AppDbContext> salesRepo,
            IBaseRepository<SaleReport, AppDbContext> salesReportRepo,
            IBaseRepository<SeriesMaster, AppDbContext> seriesMasterRepo,
            AppDbContext appDbContext)
        {
            _salesRepo = salesRepo;
            _salesReportRepo = salesReportRepo;
            _seriesMasterRepo = seriesMasterRepo;
            _appDbContext = appDbContext;
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
                           query => query.Id > 0 && query.IsDelete == false,
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
                           query => query.Id > 0 && query.IsDelete == false,
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
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;
            Expression<Func<SalesMaster, object>> salesDetails = x => x.SalesDetails.Where(s => !s.IsDelete);
            Expression<Func<SalesMaster, object>> amountReceived = m => m.AmountReceived.Where(s=>!s.IsDelete);

            if (includeDetails)
                return await _salesRepo.GetAllAsync(predicate, salesDetails, amountReceived);
            else
                return await _salesRepo.GetAllAsync(predicate);
        }

        public async Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize, bool includeDetails = false)
        {
            return await _salesRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.EntryDate,
                pageIndex, pageSize);
        }

        public async Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize, bool includeDetails = false)
        {
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id == salesId && c.IsDelete == false;
            Expression<Func<SalesMaster, object>> salesDetails = x => x.SalesDetails.Where(s => !s.IsDelete);
            Expression<Func<SalesMaster, object>> amountReceived = m => m.AmountReceived.Where(s => !s.IsDelete);

            SalesMaster salesMaster = new SalesMaster();

            if (includeDetails)
            {
                var salesWithAllDetails = await _salesRepo.GetAllAsync(predicate, salesDetails, amountReceived);

                if (salesWithAllDetails.Any())
                {
                    salesMaster = salesWithAllDetails.FirstOrDefault();
                }
            }
            else
            {
                var result = await _salesRepo.QueryAsync(
                    query => query.Id == salesId && query.IsDelete == false,
                    orderBy: c => c.EntryDate,
                    pageIndex, pageSize);

                salesMaster = result?.FirstOrDefault();
            }

            return salesMaster;
        }

        public async Task<SalesMaster> UpdateSalesAsync(SalesMaster salesMaster)
        {
            try
            {
                // get the old amount received. 

                var amountReceived = await _appDbContext.AmountReceived.Where(
                    query => salesMaster.AmountReceived.Where(x=>!x.IsDelete).Select(s => s.Id).Contains(query.Id)).ToListAsync();

                var salesDetails = await _appDbContext.SalesDetails.Where(
                    query => salesMaster.SalesDetails.Where(x => !x.IsDelete).Select(s => s.Id).Contains(query.Id)).ToListAsync();

                await _appDbContext.Database.BeginTransactionAsync();

                if (amountReceived.Any())
                    _appDbContext.AmountReceived.RemoveRange(amountReceived);

                if (salesDetails.Any())
                    _appDbContext.SalesDetails.RemoveRange(salesDetails);

                // Add the new records

                if (salesMaster.AmountReceived.Any())
                    await _appDbContext.AmountReceived.AddRangeAsync(salesMaster.AmountReceived);

                if (salesMaster.SalesDetails.Any())
                    await _appDbContext.SalesDetails.AddRangeAsync(salesMaster.SalesDetails);

                await _appDbContext.SaveChangesAsync();

                await _appDbContext.Database.CommitTransactionAsync();

                return salesMaster;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}