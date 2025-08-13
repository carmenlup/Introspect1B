namespace OrderService
{
    public class ProductCreatedEvent
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
