
using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IPOSMasterRepository
    {
        Task<List<POSMaster>> GetAllPOSMasters();
        Task<POSMaster> AddPOSMasterAsync(POSMaster pOSMaster);
        Task<POSMaster> UpdatePOSMasterAsync(POSMaster pOSMaster);
        Task<bool> DeletePOSMasterAsync(long pOSMasterId);
        Task<List<POSMaster>> GetQuery(int pageIndex, int pageSize);
        Task<POSMaster> GetQuery(long pOSMasterId, int pageIndex, int pageSize);
        Task<List<POSResponceModel>> GetPOSByUser(long UserId);
    }
}

namespace Accounts.Core.Repositories
{
    public class POSMasterRepository : IPOSMasterRepository
    {
        private readonly IBaseRepository<POSMaster, AppDbContext> _pOSMasterRepo;
        private readonly IBaseRepository<POSResponceModel, AppDbContext> _pOSResponceModelRepo;

        public POSMasterRepository(IBaseRepository<POSMaster, AppDbContext> pOSMasterRepo,
            IBaseRepository<POSResponceModel, AppDbContext> pOSResponceModelRepo)
        {
            _pOSMasterRepo = pOSMasterRepo;
            _pOSResponceModelRepo = pOSResponceModelRepo;
        }

        public async Task<POSMaster> AddPOSMasterAsync(POSMaster pOSMaster)
        {
            try
            {
                await _pOSMasterRepo.BeginTransactionAsync();

                var result = await _pOSMasterRepo.AddAsync(pOSMaster);

                await _pOSMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _pOSMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeletePOSMasterAsync(long pOSMasterId)
        {
            await _pOSMasterRepo.DeleteAsync(pOSMasterId);
            return true;
        }

        public async Task<List<POSMaster>> GetAllPOSMasters()
        {
            Expression<Func<POSMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;

            return await _pOSMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<POSMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _pOSMasterRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<POSMaster> GetQuery(long pOSMasterId, int pageIndex, int pageSize)
        {
            var result = await _pOSMasterRepo.QueryAsync(
               query => query.Id == pOSMasterId && query.IsDelete == false,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<POSMaster> UpdatePOSMasterAsync(POSMaster pOSMaster)
        {
            await _pOSMasterRepo.UpdateAsync(pOSMaster);
            return pOSMaster;
        }

        public async Task<List<POSResponceModel>> GetPOSByUser(long UserId)
        {
            //object[] paramerers = new object[] { "UserId", UserId };

            var result = await _pOSResponceModelRepo.ExecuteStoredProcedureAsync("GetPOSByUser "+ UserId);

            return result;
        }
    }
}
