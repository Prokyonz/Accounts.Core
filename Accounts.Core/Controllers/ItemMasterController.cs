using Accounts.Core.Models;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemMasterController : ControllerBase
    {
        private readonly ILogger<ItemMasterController> _logger;
        private readonly IItemMasterRepository _itemMasterRepository;

        public ItemMasterController(ILogger<ItemMasterController> logger,
            IItemMasterRepository itemMasterRepository)
        {
            _logger = logger;
            _itemMasterRepository = itemMasterRepository;
        }

        /// <summary>
        /// Read all ItemMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ItemMaster>> Get()
        {
            return await _itemMasterRepository.GetAllItemMasters();
        }

        /// <summary>
        /// Read ItemMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetItemMaster/{itemMasterId}")]
        public async Task<ItemMaster> GetRow(long itemMasterId)
        {
            return await _itemMasterRepository.GetQuery(itemMasterId, 0, 1);
        }

        /// <summary>
        /// Read list of ItemMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetItemMasterWithPagging")]
        public async Task<List<ItemMaster>> GetitemMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _itemMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Create ItemMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ItemMaster> Post(ItemMaster itemMaster)
        {
            return await _itemMasterRepository.AddItemMasterAsync(itemMaster);
        }

        /// <summary>
        /// Update ItemMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<ItemMaster> Put(ItemMaster itemMaster)
        {
            return await _itemMasterRepository.UpdateItemMasterAsync(itemMaster);
        }

        /// <summary>
        /// Delete ItemMaster.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{itemMasterId}")]
        public async Task<bool> Delete(long itemMasterId, bool isHardDelete = false)
        {
            return await _itemMasterRepository.DeleteItemMasterAsync(itemMasterId, isHardDelete);
        }
    }
}
