namespace StripeDemo.Models
{
    public struct ProductData
    {
        public string Id { get; set; }
        public string PrimaryPriceId { get; set; }
        public long? PrimaryPriceValue { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
    }
}