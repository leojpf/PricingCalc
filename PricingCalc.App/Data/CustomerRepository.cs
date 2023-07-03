using PricingCalc.App.Models;

namespace PricingCalc.App.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomer(string customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
    }

}
