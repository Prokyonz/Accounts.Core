using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Accounts.Core.Repositories
{
    public interface IUserMasterRepository
    {
        Task<List<UserMaster>> GetAllUserMasters();
        Task<UserMaster> AddUserMasterAsync(UserMaster userMaster);
        Task<UserMaster> UpdateUserMasterAsync(UserMaster userMaster);
        Task<bool> DeleteUserMasterAsync(long userMasterId, bool isHardDelete = false);
        Task<List<UserMaster>> GetQuery(int pageIndex, int pageSize);
        Task<UserMaster> GetQuery(long userMasterId, int pageIndex, int pageSize);
        Task<List<PermissionMaster>> GetMasterPermissions();
        Task<UserMaster> Login(string? mobileNo, string password, string? emailId);
        Task<List<UserReport>> UserReport(long userId, string? name);
        Task<bool> ActiveInActiveUser(long userMasterId, long userId, bool status);
    }
}

namespace Accounts.Core.Repositories
{
    public class UserMasterRepository : IUserMasterRepository
    {
        private readonly IBaseRepository<UserMaster, AppDbContext> _userMasterRepo;
        private readonly IBaseRepository<UserPermissionChild, AppDbContext> _userPermisionChild;
        private readonly IBaseRepository<POSChild, AppDbContext> _posChild;
        private readonly IBaseRepository<PermissionMaster, AppDbContext> _permissionMaster;
        private readonly IBaseRepository<UserReport, AppDbContext> _userReportRepo;
        private readonly AppDbContext _appDbContext;

        public UserMasterRepository(IBaseRepository<UserMaster, AppDbContext> userMasterRepo, 
            IBaseRepository<UserPermissionChild, AppDbContext> userPermisionChild,
            IBaseRepository<PermissionMaster, AppDbContext> permissionMaster,
            IBaseRepository<UserReport, AppDbContext> userReportRepo,
            IBaseRepository<POSChild, AppDbContext> posChild,
            AppDbContext appDbContext)
        {
            _userMasterRepo = userMasterRepo;
            _userPermisionChild = userPermisionChild;
            _permissionMaster = permissionMaster;
            _userReportRepo = userReportRepo;
            _posChild = posChild;
            _appDbContext = appDbContext;
        }

        public async Task<UserMaster> AddUserMasterAsync(UserMaster userMaster)
        {
            try
            {
                await _userMasterRepo.BeginTransactionAsync();

                var result = await _userMasterRepo.AddAsync(userMaster);

                await _userMasterRepo.CommitTransactionAsync();

                result.POSChilds.ForEach(x => { x.UserMaster = null; });

                result.Permissions.ForEach(x => { x.UserMaster = null; });

                return result;
            }
            catch (Exception)
            {
                await _userMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteUserMasterAsync(long userMasterId, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                await _userMasterRepo.DeleteAsync(userMasterId);
            }
            else
            {
                await _userMasterRepo.BeginTransactionAsync();
                var result = await _userMasterRepo.GetByIdAsync(userMasterId);
                result.UpdatedDate = DateTime.Now;
                result.IsDelete = true;
                await _userMasterRepo.CommitTransactionAsync();
            }
            return true;
        }

        public async Task<List<UserMaster>> GetAllUserMasters()
        {
            Expression<Func<UserMaster, bool>> predicate = c => c.Id > 0 && c.IsDelete == false;

            return await _userMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<UserMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _userMasterRepo.QueryAsync(
                query => query.Id > 0 && query.IsDelete == false,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<UserMaster> GetQuery(long userMasterId, int pageIndex, int pageSize)
        {
            var users = await _userMasterRepo.QueryAsync(
               query => query.Id == userMasterId && query.IsDelete == false,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            if (users?.Any() == true && users.Count > 0)
            {
                var posChilds = await _posChild.QueryAsync(
                               query => query.UserId== users[0].Id,
                               orderBy: c => c.Sr, 0, 1000);

                users[0].POSChilds = null;
                users[0].POSChilds = posChilds;

                var permissions = await _userPermisionChild.QueryAsync(
                               query => query.UserId == users[0].Id,
                               orderBy: c => c.Sr, 0, 1000);

                users[0].Permissions = null;
                users[0].Permissions = permissions;

                //return users[0];
            }

            return users?.FirstOrDefault();
        }

        public async Task<List<PermissionMaster>> GetMasterPermissions()
        {
            try
            {
                var permissions = await _permissionMaster.QueryAsync(
                               query => query.Id > 0,
                               orderBy: c => c.Id);

                return permissions;
            }
            catch (Exception ex)
            {
                throw new Exception("Not able to get the permissions, please try again later");
            }
        }

        public async Task<UserMaster> Login(string? mobileNo, string password, string? emailId)
        {
            try
            {
                var users = await _userMasterRepo.QueryAsync(
                               query => (mobileNo != null 
                                    && query.MobileNo == mobileNo 
                                    && query.Password == password
                                    && query.IsDelete == false),
                               orderBy: c => c.CreatedDate,
                               0, 10);

                if (users?.Any() == true && users.Count > 0)
                {
                    var permissions = await _userPermisionChild.QueryAsync(
                                   query => query.UserId == users[0].Id,
                                   orderBy: c => c.Sr, 0, 1000);

                    users[0].Permissions = permissions;

                    return users[0];
                }
                throw new Exception("MobileNo/EmailId or Password is incorrect");
            }
            catch (Exception ex)
            {
                throw new Exception("Not able to login at this moment, please try again later");
            }
        }

        public async Task<UserMaster> UpdateUserMasterAsync(UserMaster userMaster)
        {
            await _appDbContext.Database.BeginTransactionAsync();

            if (userMaster.POSChilds != null && userMaster.POSChilds.Any())
            {
                var posChilds = await _appDbContext.POSChild.Where(x => x.UserId == userMaster.Id).ToListAsync();

                if (posChilds.Any())
                {
                    _appDbContext.POSChild.RemoveRange(posChilds);
                }

                await _appDbContext.POSChild.AddRangeAsync(userMaster.POSChilds);
            }

            if (userMaster.Permissions != null && userMaster.Permissions.Any())
            {
                var permissions = await _appDbContext.UserPermissionChild.Where(x => x.UserId == userMaster.Id).ToListAsync();

                if (permissions.Any())
                {
                    _appDbContext.UserPermissionChild.RemoveRange(permissions);
                }

                await _appDbContext.UserPermissionChild.AddRangeAsync(userMaster.Permissions);
            }

            await _userMasterRepo.UpdateAsync(userMaster);

            await _appDbContext.SaveChangesAsync();

            await _appDbContext.Database.CommitTransactionAsync();

            return userMaster;
        }

        public async Task<List<UserReport>> UserReport(long userId, string? name)
        {
            //object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            string spName = "userReport";
            if (userId > 0)
                spName += " " + userId;
            else
                spName += " " + "NULL";
            if (!string.IsNullOrWhiteSpace(name))
                spName += " ,'" + name + "'";
            else
                spName += " ," + "NULL";
            var result = await _userReportRepo.ExecuteStoredProcedureAsync(spName);

            return result;
        }

        public async Task<bool> ActiveInActiveUser(long userMasterId, long userId, bool status)
        {
            try
            {
                await _userMasterRepo.BeginTransactionAsync();

                var result = await _userMasterRepo.QueryAsync(
                query => query.Id == userMasterId && query.IsDelete == false,
                orderBy: c => c.CreatedDate,
                0, 1);

                if (result != null)
                {
                    var userData = result.FirstOrDefault();
                    if (userData != null)
                    {
                        userData.IsActive = status;
                        userData.UpdatedBy = userId;
                        userData.UpdatedDate = DateTime.Now;
                    }
                }

                await _userMasterRepo.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
