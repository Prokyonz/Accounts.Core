using Accounts.Core.DbContext;
using Accounts.Core.Models;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ICustomerMasterRepository
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<List<Customer>> GetQuery(int pageIndex, int pageSize);
        Task<Customer> GetQuery(long customerId, int pageIndex, int pageSize);

    }
    public class CustomerMasterRepository : ICustomerMasterRepository
    {
        private readonly IBaseRepository<Customer, AppDbContext> _customerMasterRepo;

        public CustomerMasterRepository(IBaseRepository<Customer, AppDbContext> customerMasterRepo)
        {
            _customerMasterRepo = customerMasterRepo;
        }
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            try
            {
                await _customerMasterRepo.BeginTransactionAsync();

                var result = await _customerMasterRepo.AddAsync(customer);

                await _customerMasterRepo.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _customerMasterRepo.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Customer> GetQuery(long customerId, int pageIndex, int pageSize)
        {
            var result = await _customerMasterRepo.QueryAsync(
                query => query.Id == customerId,
                orderBy: c => c.FirstName,
                pageIndex, pageSize);

            return result?.FirstOrDefault();
        }

        public async Task<List<Customer>> GetQuery(int pageIndex, int pageSize)
        {
            return await _customerMasterRepo.QueryAsync(
                query => query.Id > 0,
                orderBy: c => c.FirstName,
                pageIndex, pageSize);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            Expression<Func<Customer, bool>> predicate = c => c.Id > 0;

            return await _customerMasterRepo.GetAllAsync(predicate);
        }
    }
}
