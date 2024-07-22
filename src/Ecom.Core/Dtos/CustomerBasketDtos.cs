using Ecom.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Dtos
{
    public class CustomerBasketDtos
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemsDtos> BasketItems { get; set; } = new List<BasketItemsDtos>();

    }
}
