using BasketService.Infrastructure.Context;
using BasketService.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketService.Model.Services
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string UserId);
    }

    public class BasketServices:IBasketService
    {
        private readonly BasketDatabaseContext databaseContext;

        public BasketServices(BasketDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public BasketDto GetOrCreateBasketForUser(string UserId)
        {
            var basket = databaseContext.Basket
                         .Include(p => p.Items)
                         .SingleOrDefault(c => c.UserId == UserId);
            if(basket == null)
            {
               return createBasketForUser(UserId);
            }

            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                Items = basket.Items.Select(item=>new BasketItemsDto
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    ProductId= item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                    
                } ).ToList(),
                 
            };
        }

        public BasketDto createBasketForUser(string UserId)
        {
            Basket basket = new Basket(UserId);
            databaseContext.Basket.Add(basket);
            databaseContext.SaveChanges();
            return new BasketDto
            {
                UserId = basket.UserId,
                Id= basket.Id
            };
        }
    }

    public class BasketDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public List<BasketItemsDto> Items { get; set; } = new List<BasketItemsDto>();

        public int Total ()
        {
            if(Items.Count >0)
            {
                var total = Items.Sum(c=>c.UnitPrice * c.Quantity);
                return total;
            }
            return 0;
        }

    }

    public class BasketItemsDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
     

    }
}
