using Microsoft.AspNetCore.Mvc;
using PricingCalc.App.Models;

namespace PricingCalc.App.Controllers
{
    public class PricingController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;

        public PricingController(ICustomerRepository customerRepository, IServiceRepository serviceRepository)
        {
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetPrice(string customerId, DateTime start, DateTime end)
        {
            var customer = await _customerRepository.GetCustomer(customerId);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            decimal totalCost = 0;
            decimal price = 0;
            foreach (var serviceId in customer.ServicePrices.Keys)
            {
                var service = await _serviceRepository.GetService(serviceId);
                if (service == null)
                {
                    continue;
                }

                var days = CalculateDays(service, start, end);
                days += customer.FreeDays == 0 ? 0 : (-1 * customer.FreeDays);
                var costPerDay = customer.ServicePrices[serviceId];
                var discount = customer.ServiceDiscounts.ContainsKey(serviceId) ? customer.ServiceDiscounts[serviceId] : 0;
                price = days * costPerDay * (1 - discount / 100m);
                var discountPerday = CalculateDaysWithDiscount(days, service.StartDateDiscount, service.EndDateDiscount, discount, costPerDay);
                price += discountPerday == 0 ? 0 : (-1 * discountPerday);
                totalCost += price;
            }

            return Ok(totalCost);
        }

        private decimal CalculateDaysWithDiscount(int days, DateTime? startDateDiscount, DateTime? endDateDiscount, int discount, decimal costPerDay)
        {
            if (startDateDiscount == null || endDateDiscount == null)
                return 0;

            var dateDifference = (endDateDiscount.Value - startDateDiscount.Value).Days;

            var DiscountOnly = dateDifference * costPerDay * (1 - discount / 100m);

            return DiscountOnly;
        }

        private int CalculateDays(Service service, DateTime start, DateTime end)
        {
            int totalDays = 0;
            for (var day = start; day <= end; day = day.AddDays(1))
            {
                if (service.IsWorkingDayOnly && (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday))
                {
                    continue;
                }

                totalDays++;
            }

            return totalDays;
        }
    }
}
