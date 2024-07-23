using Ecom.Core.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class OrderDtos
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDtos ShipToAddress { get; set; }

    }
}