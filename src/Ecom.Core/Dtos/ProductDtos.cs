using Ecom.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.Dtos
{
    public class BaseClassProduct
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0,9999,ErrorMessage ="Price Limited By [0] And [1]")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Price Must Be Number")]

        public decimal Price { get; set; }
    }
    public class ProductDtos : BaseClassProduct
    {

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }

    }
    public class AddProductDtos : BaseClassProduct
    {
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }

    }
    public class UpdateProductDtos : BaseClassProduct 
    {
        public int CategoryId { get; set; }
        //public string ProductOldPicture { get; set; }

        public IFormFile Image { get; set; }
    }

}
