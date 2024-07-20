using Ecom.Core.Dtos;
using Ecom.Core;
using Ecom.Core.Entities;
using Ecom.Core.Sharing;




namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<ProductDtos>> GetAllAsync(ProductParams ProdParam);
        Task<bool> AddAsync(AddProductDtos ProdDto);
        Task<bool> UpdateAsync(int id ,UpdateProductDtos ProdDto);
        Task<bool> DeleteAsyncWithPicture(int id);
        Task<int> CountProductsAsync(ProductParams ProdParam);


    }
}
