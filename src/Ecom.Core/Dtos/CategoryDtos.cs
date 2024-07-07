using System.ComponentModel.DataAnnotations;

namespace Ecom.Core.Dtos
{
    public class CategoryDtos
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

    }
    public class ListingCategoryDtos : CategoryDtos
    {
        public int Id { get; set; }

    }
}
