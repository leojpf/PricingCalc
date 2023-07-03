namespace PricingCalc.App.Models
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomer(string customerId);
    }
}
