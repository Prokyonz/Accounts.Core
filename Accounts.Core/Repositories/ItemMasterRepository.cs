using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IItemMasterRepository
    {
        Task<List<ItemMaster>> GetAllItemMasters();
        Task<ItemMaster> AddItemMasterAsync(ItemMaster itemMaster);
        Task<ItemMaster> UpdateItemMasterAsync(ItemMaster itemMaster);
        Task<bool> DeleteItemMasterAsync(long itemMasterId);
        Task<List<ItemMaster>> GetQuery(int pageIndex, int pageSize);
        Task<ItemMaster> GetQuery(long itemMasterId, int pageIndex, int pageSize);
    }
}

namespace Accounts.Core.Repositories
{
    public class ItemMasterRepository : IItemMasterRepository
    {
        private readonly IBaseRepository<ItemMaster, AppDbContext> _itemMasterRepo;

        public ItemMasterRepository(IBaseRepository<ItemMaster, AppDbContext> itemMasterRepo)
        {
            _itemMasterRepo = itemMasterRepo;
        }

        public async Task<ItemMaster> AddItemMasterAsync(ItemMaster itemMaster)
        {
            try
            {
                await _itemMasterRepo.BeginTransactionAsync();

                var result = await _itemMasterRepo.AddAsync(itemMaster);

                await _itemMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _itemMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteItemMasterAsync(long itemMasterId)
        {
            await _itemMasterRepo.DeleteAsync(itemMasterId);
            return true;
        }

        public async Task<List<ItemMaster>> GetAllItemMasters()
        {
            Expression<Func<ItemMaster, bool>> predicate = c => c.Id > 0;

            return await _itemMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<ItemMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _itemMasterRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<ItemMaster> GetQuery(long itemMasterId, int pageIndex, int pageSize)
        {
            var result = await _itemMasterRepo.QueryAsync(
               query => query.Id == itemMasterId,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<ItemMaster> UpdateItemMasterAsync(ItemMaster itemMaster)
        {
            await _itemMasterRepo.UpdateAsync(itemMaster);
            return itemMaster;
        }
    }
}
