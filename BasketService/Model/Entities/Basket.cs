namespace BasketService.Model.Entities
{
    public class Basket
    {
        public Basket(string userid)
        {
            UserId = userid;
        }
        public Basket()
        {
            
        }
        public Guid Id { get; set; }
        public string UserId { get; private set; }

        public List<BasketItems> Items { get; set; } =new List<BasketItems>();
  
    }

    public class BasketItems
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }        
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public Basket Basket { get; set; }
        public Guid BasketId { get; set; }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

       
    }
}
