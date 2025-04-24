using AutoMapper;
using BasketService.Infrastructure.Context;
using BasketService.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasketService.Model.Services
{
    public interface IBasketService
    {
        BasketDto GetOrCreateBasketForUser(string UserId);
        BasketDto GetBasket(string UserId);
        void AddItemsToBasket(AddItemsToBasketDto basketItems);
        void RemoveItemsFromBasket(Guid itemId);
        void SetItemQuantity(Guid itemId,int quantity);
        void TransferBasket(string AnonymousId, string UserId);
    }

    public class BasketServices:IBasketService
    {
        private readonly BasketDatabaseContext databaseContext;
        private readonly IMapper mapper;

        public BasketServices(BasketDatabaseContext databaseContext,IMapper mapper)
        {
            this.databaseContext = databaseContext;
            this.mapper = mapper;
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

        public BasketDto GetBasket(string UserId)
        {
            var basket = databaseContext.Basket
                         .Include(p => p.Items)
                         .SingleOrDefault(c => c.UserId == UserId);
            if (basket == null)
                return null;
            return new BasketDto
            {
                Id = basket.Id,
                UserId = basket.UserId,
                Items = basket.Items.Select(item => new BasketItemsDto
                {
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice

                }).ToList(),
            };
        }

        public void AddItemsToBasket(AddItemsToBasketDto basketItems)
        {
            var basket = databaseContext.Basket.FirstOrDefault(c=>c.Id==basketItems.BasketId);

            if (basket == null)
                throw new Exception("basket not found");

            var basketItem = mapper.Map<BasketItems>(basketItems);

            basket.Items.Add(basketItem);
            databaseContext.SaveChanges();

        }

        public void RemoveItemsFromBasket(Guid itemId)
        {
            var item = databaseContext.BasketItems.SingleOrDefault(c => c.Id == itemId);

            if (item == null)
                throw new Exception("item not found");
            databaseContext.BasketItems.Remove(item);
            databaseContext.SaveChanges();
        }

        public void SetItemQuantity(Guid itemId, int quantity)
        {
            var item = databaseContext.BasketItems.SingleOrDefault(c => c.Id == itemId);

            if (item == null)
                throw new Exception("basket not found");
            item.SetQuantity(quantity);
            databaseContext.SaveChanges();
        }

        public void TransferBasket(string AnonymousId, string UserId)
        {
            var anonymousBasket = databaseContext.Basket
                       .Include(p => p.Items)
                       .SingleOrDefault(c => c.UserId == AnonymousId);
            if (anonymousBasket == null) return;
            var userBasket = databaseContext.Basket.SingleOrDefault(c => c.UserId == UserId);
            if (userBasket == null)
            {
                userBasket = new Basket(UserId);
                databaseContext.Basket.Add(userBasket);
            }

            foreach (var item in anonymousBasket.Items)
            {
                userBasket.Items.Add(new BasketItems
                {
                    ImageUrl=item.ImageUrl,
                    ProductName=item.ProductName,
                    Quantity=item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ProductId=item.ProductId                    
                });                
            }
            databaseContext.Basket.Remove(anonymousBasket);
            databaseContext.SaveChanges();


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
    public class AddItemsToBasketDto
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        
     

    }
}
