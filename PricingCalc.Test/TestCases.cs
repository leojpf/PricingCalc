using Microsoft.AspNetCore.Mvc;
using Moq;
using PricingCalc.App.Controllers;
using PricingCalc.App.Models;

namespace PricingCalc.Test
{
    public class TestCases
    {
        [Fact]
        public async Task TestCustomerX()
        {
            // Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var serviceRepository = new Mock<IServiceRepository>();

            var customerX = new Customer
            {
                Id = "X",
                ServicePrices = new Dictionary<string, decimal> { { "A", 0.2m }, { "C", 0.4m } },
                ServiceDiscounts = new Dictionary<string, int> { { "C", 20 } },
                ServiceStartDates = new Dictionary<string, DateTime> { { "A", new DateTime(2019, 9, 20) }, { "C", new DateTime(2019, 9, 20) } },
                FreeDays = 0
            };

            var serviceA = new Service { Id = "A", CostPerDay = 0.2m, IsWorkingDayOnly = true };
            var serviceC = new Service { Id = "C", CostPerDay = 0.4m, IsWorkingDayOnly = false, StartDateDiscount = new DateTime(2019, 09, 22), EndDateDiscount = new DateTime(2019, 09, 24) };

            customerRepository.Setup(r => r.GetCustomer("X")).ReturnsAsync(customerX);
            serviceRepository.Setup(r => r.GetService("A")).ReturnsAsync(serviceA);
            serviceRepository.Setup(r => r.GetService("C")).ReturnsAsync(serviceC);

            var controller = new PricingController(customerRepository.Object, serviceRepository.Object);

            // Act
            var result = await controller.GetPrice("X", new DateTime(2019, 9, 20), new DateTime(2019, 10, 1));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var price = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(4.80m, price); // Expected price
        }
        [Fact]
        public async Task TestCustomerY()
        {
            // Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var serviceRepository = new Mock<IServiceRepository>();

            var customerY = new Customer
            {
                Id = "Y",
                ServicePrices = new Dictionary<string, decimal> { { "B", 0.24m }, { "C", 0.4m } },
                ServiceDiscounts = new Dictionary<string, int> { { "B", 30 }, { "C", 30 } },
                ServiceStartDates = new Dictionary<string, DateTime> { { "B", new DateTime(2018, 1, 1) }, { "C", new DateTime(2018, 1, 1) } },
                FreeDays = 200
            };

            var serviceB = new Service { Id = "B", CostPerDay = 0.24m, IsWorkingDayOnly = true };
            var serviceC = new Service { Id = "C", CostPerDay = 0.4m, IsWorkingDayOnly = false };

            customerRepository.Setup(r => r.GetCustomer("Y")).ReturnsAsync(customerY);
            serviceRepository.Setup(r => r.GetService("B")).ReturnsAsync(serviceB);
            serviceRepository.Setup(r => r.GetService("C")).ReturnsAsync(serviceC);

            var controller = new PricingController(customerRepository.Object, serviceRepository.Object);

            // Act
            var result = await controller.GetPrice("Y", new DateTime(2018, 1, 1), new DateTime(2019, 10, 1));

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var price = Assert.IsType<decimal>(okResult.Value);
            Assert.Equal(166.096m, price); // Expected price
        }
    }
}
