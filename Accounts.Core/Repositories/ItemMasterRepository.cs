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
        Task<bool> DeleteItemMasterAsync(long itemMasterId, bool isHardDelete = false);
        Task<List<ItemMaster>> GetQuery(int pageIndex, int pageSize);
        Task<ItemMaster> GetQuery(long itemMasterId, int pageIndex, int pageSize);
        Task<bool> ActiveInActiveItem(long itemMasterId, long userId, bool status);
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

        public async Task<bool> DeleteItemMasterAsync(long itemMasterId, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                await _itemMasterRepo.DeleteAsync(itemMasterId);
            }
            else
            {
                await _itemMasterRepo.BeginTransactionAsync();
                var result = await _itemMasterRepo.GetByIdAsync(itemMasterId);
                result.UpdatedDate = DateTime.Now;
                result.IsDelete = true;
                await _itemMasterRepo.CommitTransactionAsync();
            }
            return true;
        }

        public async Task<List<ItemMaster>> GetAllItemMasters()
        {
            Expression<Func<ItemMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;

            return await _itemMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<ItemMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _itemMasterRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<ItemMaster> GetQuery(long itemMasterId, int pageIndex, int pageSize)
        {
            var result = await _itemMasterRepo.QueryAsync(
               query => query.Id == itemMasterId && query.IsDelete == false,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<ItemMaster> UpdateItemMasterAsync(ItemMaster itemMaster)
        {
            await _itemMasterRepo.UpdateAsync(itemMaster);
            return itemMaster;
        }

        public async Task<bool> ActiveInActiveItem(long itemMasterId, long userId, bool status)
        {
            try
            {
                await _itemMasterRepo.BeginTransactionAsync();

                var result = await _itemMasterRepo.QueryAsync(
                query => query.Id == itemMasterId && query.IsDelete == false,
                orderBy: c => c.CreatedDate,
                0, 1);

                if (result != null)
                {
                    var itemData = result.FirstOrDefault();
                    if (itemData != null)
                    {
                        itemData.IsActive = status;
                        itemData.UpdatedBy = userId;
                        itemData.UpdatedDate = DateTime.Now;
                    }
                }

                await _itemMasterRepo.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
