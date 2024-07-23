using Ecom.Core.Entities;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uow;
        private readonly ApplicationDbContext _context;

        public OrderServices(IUnitOfWork uow,ApplicationDbContext context)
        {
            _uow = uow;
            _context = context;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var basket = await _uow.BasketRepository.GetBasketAsync(basketId);
                var items = new List<OrderItem>();

                // Sequentially fetch product items to avoid concurrency issues
                foreach (var basketItem in basket.BasketItems)
                {
                    var productItem = await _uow.ProductRepository.GetByIdAsync(basketItem.Id);
                    var productItemOrdered = new ProductItemOrderd(productItem.Id, productItem.Name, productItem.ProductPicture);
                    var orderItem = new OrderItem(productItemOrdered, basketItem.Price, basketItem.Quantity);
                    items.Add(orderItem);
                }

                var deliveryMethod = await _context.DeliveryMethods
                                                   .FirstOrDefaultAsync(x => x.Id == deliveryMethodId);

                var subTotal = items.Sum(x => x.Price * x.Quantity);
                var order = new Order(buyerEmail, shipAddress, deliveryMethod, items, subTotal);

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
           => await _context.DeliveryMethods.ToListAsync();


        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _context.Orders
                    .Where(x => x.Id == id && x.BuyerEmail == buyerEmail)
                    .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                    .Include(x => x.DeliveryMethod)
                    .OrderByDescending(x => x.OrderDate)
                    .FirstOrDefaultAsync();

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var order = await _context.Orders
                    .Where(x => x.BuyerEmail == buyerEmail)
                    .Include(x => x.OrderItems).ThenInclude(x => x.ProductItemOrderd)
                    .Include(x => x.DeliveryMethod)
                    .OrderByDescending(x => x.OrderDate)
                    .ToListAsync();

            return order;
        }
    }
}
