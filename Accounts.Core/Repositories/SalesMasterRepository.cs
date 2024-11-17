using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ISalesMasterRepository
    {
        Task<List<SalesMaster>> GetAllSales();
        Task<SalesMaster> AddSalesAsync(SalesMaster salesMaster);
        Task<SalesMaster> UpdateSalesAsync(SalesMaster  salesMaster);
        Task<bool> DeleteSalesAsync(long salesId);
        Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize);
        Task<SalesMaster> GetQuery(long stockId, int pageIndex, int pageSize);
    }

    public class SalesMasterRepository : ISalesMasterRepository
    {
        private readonly IBaseRepository<SalesMaster, AppDbContext> _salesRepo;

        public SalesMasterRepository(IBaseRepository<SalesMaster, AppDbContext> salesRepo)
        {
            _salesRepo = salesRepo;
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

        public async Task<List<SalesMaster>> GetAllSales()
        {
            Expression<Func<SalesMaster, bool>> predicate = c => c.Id > 0;

            return await _salesRepo.GetAllAsync(predicate);
        }

        public async Task<List<SalesMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _salesRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.EntryDate,
                pageIndex, pageSize);
        }

        public async Task<SalesMaster> GetQuery(long salesId, int pageIndex, int pageSize)
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
