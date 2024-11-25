using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface IUserMasterRepository
    {
        Task<List<UserMaster>> GetAllUserMasters();
        Task<UserMaster> AddUserMasterAsync(UserMaster userMaster);
        Task<UserMaster> UpdateUserMasterAsync(UserMaster userMaster);
        Task<bool> DeleteUserMasterAsync(long userMasterId);
        Task<List<UserMaster>> GetQuery(int pageIndex, int pageSize);
        Task<UserMaster> GetQuery(long userMasterId, int pageIndex, int pageSize);
        Task<UserMaster> Login(string? mobileNo, string password, string? emailId);
    }
}

namespace Accounts.Core.Repositories
{
    public class UserMasterRepository : IUserMasterRepository
    {
        private readonly IBaseRepository<UserMaster, AppDbContext> _userMasterRepo;

        public UserMasterRepository(IBaseRepository<UserMaster, AppDbContext> userMasterRepo)
        {
            _userMasterRepo = userMasterRepo;
        }

        public async Task<UserMaster> AddUserMasterAsync(UserMaster userMaster)
        {
            try
            {
                await _userMasterRepo.BeginTransactionAsync();

                var result = await _userMasterRepo.AddAsync(userMaster);

                await _userMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _userMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteUserMasterAsync(long userMasterId)
        {
            await _userMasterRepo.DeleteAsync(userMasterId);
            return true;
        }

        public async Task<List<UserMaster>> GetAllUserMasters()
        {
            Expression<Func<UserMaster, bool>> predicate = c => c.Id > 0;

            return await _userMasterRepo.GetAllAsync(predicate);
        }

        public async Task<List<UserMaster>> GetQuery(int pageIndex, int pageSize)
        {
            return await _userMasterRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.CreatedDate ?? DateTime.Now,
                pageIndex, pageSize);
        }

        public async Task<UserMaster> GetQuery(long userMasterId, int pageIndex, int pageSize)
        {
            var result = await _userMasterRepo.QueryAsync(
               query => query.Id == userMasterId,
               orderBy: c => c.CreatedDate,
               pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<UserMaster> Login(string? mobileNo, string password, string? emailId)
        {
            try
            {
                var users = await _userMasterRepo.QueryAsync(
                               query => (query.MobileNo == mobileNo && query.Password == password) || (query.EmailId == emailId && query.Password == password),
                               orderBy: c => c.CreatedDate,
                               0, 10);

                if(users?.Any() == true && users.Count > 0)
                {
                    return users[0];
                }
                throw new Exception("MobileNo/EmailId or Password is incorrect");
            }
            catch(Exception ex)
            {
                throw new Exception("Not able to login at this moment, please try again later");
            }
        }

        public async Task<UserMaster> UpdateUserMasterAsync(UserMaster userMaster)
        {
            await _userMasterRepo.UpdateAsync(userMaster);
            return userMaster;
        }
    }
}