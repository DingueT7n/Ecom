using Ecom.Core.Dtos;
using Ecom.Core;
using Ecom.Core.Entities;




namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDtos ProdDto);
        Task<bool> UpdateAsync(int id ,UpdateProductDtos ProdDto);
        Task<bool> DeleteAsyncWithPicture(int id);


    }
}
