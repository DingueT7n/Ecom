using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public BasketsController(IUnitOfWork uow)
        {
            _uow = uow;
        }



        [HttpGet("Get-Basket-Item/{id}")]
        public async Task<IActionResult> GetBasketItem(string id) {
            var _Basket = await _uow.BasketRepository.GetBasketAsync(id);
            return Ok(_Basket ?? new CustomerBasket(id));
        }


        [HttpPost("Update-Basket-Item")]
        public async Task<IActionResult> UpdateBasketItem(CustomerBasketDtos customerbasket)
        {
            var _result = new CustomerBasket
            {
                BasketItems = customerbasket.BasketItems.Select(item => new BasketItems
                {
                 Id = item.Id,
                 ProductName = item.ProductName,
                 ProductPicture = item.ProductPicture,
                 Price = item.Price,
                 Quantity = item.Quantity,
                 Category = item.Category,

                }).ToList(),

                Id = customerbasket.Id
            };
            var _Basket = await _uow.BasketRepository.UpdateBasketAsync(_result);
            return Ok(_Basket);
        }



        [HttpDelete("Delete-Basket-Item/{id}")]
        public async Task<IActionResult> DeleteBasketItem(string id)
        {
            var _Basket = await _uow.BasketRepository.DeleteBasketAsync(id);
            return Ok(_Basket);
        }

    }
}
