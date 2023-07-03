namespace PricingCalc.App.Models
{
    public interface IServiceRepository
    {
        Task<Service> GetService(string serviceId);
    }
}
