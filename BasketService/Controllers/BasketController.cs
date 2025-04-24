using BasketService.Model.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }
        // GET: api/<BasketController>
        [HttpGet]
        public IActionResult Get(string userId)
        {
            var basket = basketService.GetOrCreateBasketForUser(userId);
            return Ok(basket);
        }

       

        // POST api/<BasketController>
        [HttpPost]
        public IActionResult AddItemToBasket(AddItemsToBasketDto addItemsToBasket,string userId)
        {
            var basket = basketService.GetOrCreateBasketForUser(userId);
            addItemsToBasket.BasketId = basket.Id;
            basketService.AddItemsToBasket(addItemsToBasket);
            var newBasket = basketService.GetBasket(userId);
            return Ok();
        }

        // PUT api/<BasketController>/5
        [HttpPut]
        public IActionResult SetQuantity(Guid itemId, int quantity)
        {
            basketService.SetItemQuantity(itemId, quantity);
            return Ok("successfully updated");
        }

        // DELETE api/<BasketController>/5
        [HttpDelete]
        public IActionResult DeleteItemsFromBasket(Guid id)
        {
            basketService.RemoveItemsFromBasket(id);
            return Ok("item deleted successfully");

        }
    }
}
