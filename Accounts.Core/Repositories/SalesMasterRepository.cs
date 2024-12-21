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
        Task<List<SaleReport>> SalesReport();
    }

    public class SalesMasterRepository : ISalesMasterRepository
    {
        private readonly IBaseRepository<SalesMaster, AppDbContext> _salesRepo;
        private readonly IBaseRepository<SaleReport, AppDbContext> _salesReportRepo;

        public SalesMasterRepository(IBaseRepository<SalesMaster, AppDbContext> salesRepo, IBaseRepository<SaleReport, AppDbContext> salesReportRepo)
        {
            _salesRepo = salesRepo;
            _salesReportRepo = salesReportRepo;
        }

        public async Task<List<SaleReport>> SalesReport()
        {
            object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            var result = await _salesReportRepo.ExecuteStoredProcedureAsync("salesReport", paramerers);

            return result;
        }

        public async Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster)
        {
            try
            {
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
