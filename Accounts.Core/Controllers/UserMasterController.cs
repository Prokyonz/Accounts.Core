using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserMasterController : ControllerBase
    {
        private readonly ILogger<UserMasterController> _logger;
        private readonly IUserMasterRepository _userMasterRepository;

        public UserMasterController(ILogger<UserMasterController> logger,
            IUserMasterRepository userMasterRepository)
        {
            _logger = logger;
            _userMasterRepository = userMasterRepository;
        }

        /// <summary>
        /// Read all UserMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserMaster>> Get()
        {
            return await _userMasterRepository.GetAllUserMasters();
        }

        [HttpGet("Login")]
        public async Task<UserMaster> Login(string? mobileNo, string password, string? emailId)
        {
            return await _userMasterRepository.Login(mobileNo, password, emailId);
        }

        /// <summary>
        /// Read UserMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserMaster/{userMasterId}")]
        public async Task<UserMaster> GetRow(long userMasterId, int pageIndex, int pageSize)
        {
            return await _userMasterRepository.GetQuery(userMasterId, 0, 1);
        }

        /// <summary>
        /// Read list of UserMaster from table.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserMasterWithPagging")]
        public async Task<List<UserMaster>> GetuserMasterWithPagging(int pageIndex, int pageSize)
        {
            var result = await _userMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// Get the permission list.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPermissionMasters")]
        public async Task<List<PermissionMaster>> GetPermissionMasters()
        {
            var result = await _userMasterRepository.GetMasterPermissions();
            return result;
        }

        /// <summary>
        /// Create UserMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserMaster> Post(UserMaster userMaster)
        {
            userMaster.Id = null;
            return await _userMasterRepository.AddUserMasterAsync(userMaster);
        }

        /// <summary>
        /// Update UserMaster.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<UserMaster> Put(UserMaster userMaster)
        {
            return await _userMasterRepository.UpdateUserMasterAsync(userMaster);
        }

        /// <summary>
        /// Delete UserMaster.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> Delete(long userMasterId)
        {
            return await _userMasterRepository.DeleteUserMasterAsync(userMasterId);
        }

        [HttpGet("UserReport")]
        public async Task<List<UserReport>> GetUserReport([FromQuery] long userId, [FromQuery] string? name)
        {
            return await _userMasterRepository.UserReport(userId, name);
        }
    }
}
