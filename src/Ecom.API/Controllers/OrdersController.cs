using Ecom.API.Errors;
using Ecom.Core.Dtos;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IOrderServices _orderServices;

        public OrdersController(IUnitOfWork uow,IOrderServices orderServices)
        {
            _uow = uow;
            _orderServices = orderServices;
        }
        [HttpPost("Create-Order")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDtos orderdto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var address = new ShipAddress
            {
                FirstName = orderdto.ShipToAddress.FirstName,
                LastName = orderdto.ShipToAddress.LastName,
                City = orderdto.ShipToAddress.City,
                State = orderdto.ShipToAddress.State,
                Street = orderdto.ShipToAddress.Street,
                ZipCode = orderdto.ShipToAddress.ZipCode,
            };

            var order = await _orderServices.CreateOrderAsync(email, orderdto.DeliveryMethodId, orderdto.BasketId, address);

            if (order is null) return BadRequest(new BaseCommuneResponse(400, "Error While Creating Order"));

            return Ok(order);

        }

        [HttpGet("Get-Orders-For-User")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDtos>>> GetOrdersForUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var orders = await _orderServices.GetOrdersForUserAsync(email);
            var result = orders.Select(order => new OrderToReturnDtos
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShipToAddress = order.ShipToAddress,
                DeliveryMethod = order.DeliveryMethod.ShortName, // Assuming DeliveryMethod has a Name property
                ShippingPrice = order.DeliveryMethod.Price, // Assuming DeliveryMethod has a Price property
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDtos
                {
                    ProductItemId = orderItem.ProductItemOrderd.ProductItemId,
                    ProductItemName = orderItem.ProductItemOrderd.ProductItemName,
                    PictureUrl = orderItem.ProductItemOrderd.PictureUrl,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                }).ToList(),
                Subtotal = order.Subtotal,
                Total = order.GetTotal(),
                OrderStatus = order.OrderStatus.ToString()
            }).ToList();
       

            return Ok(result);
        }

        [HttpGet("Get-Order-By-Id/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new BaseCommuneResponse(401));
            }

            var order = await _orderServices.GetOrderByIdAsync(id, email);
            if (order == null)
            {
                return NotFound(new BaseCommuneResponse(404));
            }

            var result = new OrderToReturnDtos
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShipToAddress = order.ShipToAddress,
                DeliveryMethod = order.DeliveryMethod.ShortName, // Assuming DeliveryMethod has a Name property
                ShippingPrice = order.DeliveryMethod.Price, // Assuming DeliveryMethod has a Price property
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemDtos
                {
                    ProductItemId = orderItem.ProductItemOrderd.ProductItemId,
                    ProductItemName = orderItem.ProductItemOrderd.ProductItemName,
                    PictureUrl = orderItem.ProductItemOrderd.PictureUrl,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                }).ToList(),
                Subtotal = order.Subtotal,
                Total = order.GetTotal(),
                OrderStatus = order.OrderStatus.ToString()
            };

            return Ok(result);
        }
        [HttpGet("Get-Delivery-Methods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            return Ok(await _orderServices.GetDeliveryMethodsAsync());
        }




    }
}
