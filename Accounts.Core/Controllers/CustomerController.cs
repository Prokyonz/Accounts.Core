using Accounts.Core.Models;
using Accounts.Core.Models.Response;
using Accounts.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {      
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerMasterRepository _customerMasterRepository;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerMasterRepository customerMasterRepository)
        {
            _logger = logger;
            _customerMasterRepository = customerMasterRepository;
        }

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            var result = await _customerMasterRepository.GetAllCustomers();
            return result;
        }

        [HttpGet("GetCustomer/{customerId}")]
        public async Task<Customer> GetRow(long customerId, int pageIndex, int pageSize)
        {
            var result = await _customerMasterRepository.GetQuery(customerId, 0, 1);
            return result;
        }

        [HttpGet("GetCustomersWithPagging")]
        public async Task<List<Customer>> GetCustomerWithPagging(int pageIndex, int pageSize)
        {
            var result = await _customerMasterRepository.GetQuery(pageIndex, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<Customer> Post(Customer customer)
        {
            try
            {
                return await _customerMasterRepository.AddCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<Customer> Put(Customer customer)
        {
            try
            {
                return await _customerMasterRepository.UpdateCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<bool> Delete(long customerId, bool isHardDelete = false)
        {
            try
            {
                return await _customerMasterRepository.DeleteCustomerAsync(customerId, isHardDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("CustomerReport")]
        public async Task<List<CustomerReport>> GetCustomerReport([FromQuery] long userId, [FromQuery] string? name)
        {
            return await _customerMasterRepository.CustomerReport(userId, name);
        }
    }
}