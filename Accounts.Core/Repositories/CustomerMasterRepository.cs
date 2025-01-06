using Accounts.Core.DbContext;
using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using BaseClassLibrary.Interface;
using System.Linq.Expressions;

namespace Accounts.Core.Repositories
{
    public interface ICustomerMasterRepository
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(long customerId);
        Task<List<Customer>> GetQuery(int pageIndex, int pageSize);
        Task<Customer> GetQuery(long customerId, int pageIndex, int pageSize);
        Task<List<CustomerReport>> CustomerReport(long userId, string? name);
    }
    public class CustomerMasterRepository : ICustomerMasterRepository
    {
        private readonly IBaseRepository<Customer, AppDbContext> _customerMasterRepo;
        private readonly IBaseRepository<CustomerReport, AppDbContext> _customberReportRepo;

        public CustomerMasterRepository(IBaseRepository<Customer, AppDbContext> customerMasterRepo, IBaseRepository<CustomerReport, AppDbContext> customberReportRepo)
        {
            _customerMasterRepo = customerMasterRepo;
            _customberReportRepo = customberReportRepo;
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

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            await _customerMasterRepo.UpdateAsync(customer);
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(long customerId)
        {
            await _customerMasterRepo.DeleteAsync(customerId);
            return true;
        }

        public async Task<List<CustomerReport>> CustomerReport(long userId, string? name)
        {
            //object[] paramerers = new object[] { "Id", 1, "Name", "Abhishek" };

            string spName = "customberReport";
            if (userId > 0)
                spName += " " + userId;
            else
                spName += " " + "NULL";
            if (!string.IsNullOrWhiteSpace(name))
                spName += " ,'" + name + "'";
            else
                spName += " ," + "NULL";
            var result = await _customberReportRepo.ExecuteStoredProcedureAsync(spName);

            return result;
        }
    }
}
