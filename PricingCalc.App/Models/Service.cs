namespace PricingCalc.App.Models
{
    public class Service
    {
        public string Id { get; set; }
        public decimal CostPerDay { get; set; }
        public bool IsWorkingDayOnly { get; set; }
        public DateTime? StartDateDiscount { get; set; }
        public DateTime? EndDateDiscount { get; set; }
    }
}
