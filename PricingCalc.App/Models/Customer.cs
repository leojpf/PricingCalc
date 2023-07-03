namespace PricingCalc.App.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public Dictionary<string, decimal> ServicePrices { get; set; }
        public Dictionary<string, int> ServiceDiscounts { get; set; }
        public Dictionary<string, DateTime> ServiceStartDates { get; set; }
        public int FreeDays { get; set; }
    }
}
