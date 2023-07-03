using PricingCalc.App.Models;

namespace PricingCalc.App.Data
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Service> GetService(string serviceId)
        {
            return await _context.Services.FindAsync(serviceId);
        }
    }

}
