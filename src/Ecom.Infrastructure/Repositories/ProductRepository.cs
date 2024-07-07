using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
      

        public ProductRepository(ApplicationDbContext context,IFileProvider fileProvider) : base(context)
        {
          _context = context;
            _fileProvider = fileProvider;
           
        }

        public async Task<bool> AddAsync(AddProductDtos ProdDto)
        {
            if (ProdDto.Image is not null)
            {
                var root = "/images/Products/";
                var ProductName = $"{Guid.NewGuid()}"+ProdDto.Image.FileName;
                if (ProductName.Contains("+"))
                {
                    ProductName = ProductName.Replace("+", "");
                }
                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot"+root);
                }
                var src =  root + ProductName;
                var picInfo = _fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                using (var fileStream = new FileStream(rootPath, FileMode.Create))
                {
                    
                    await ProdDto.Image.CopyToAsync(fileStream);
                }
                var NewProduct = new Product
                {
                    Name = ProdDto.Name,
                    Description = ProdDto.Description,
                    Price = ProdDto.Price,
                    CategoryId = ProdDto.CategoryId,
                    
                };
                NewProduct.ProductPicture = src;
                await _context.Products.AddAsync(NewProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false; 
        }

        public async Task<bool> DeleteAsyncWithPicture(int id)
        {
            var productToDelete = await _context.Products.FindAsync( id);
            if (productToDelete is not null)
            {
                if (!string.IsNullOrEmpty(productToDelete.ProductPicture))
                {
                    var picInformation = _fileProvider.GetFileInfo(productToDelete.ProductPicture);
                    var picPath = picInformation.PhysicalPath;
                    System.IO.File.Delete(picPath);


                }
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
                return true;

            }
            return false;
        }

        public async Task<bool> UpdateAsync(int id,UpdateProductDtos ProdDto)
        {

            var OldProduct=await _context.Products.FindAsync( id);
            if (OldProduct is not null) 
            {
                var src = "";
                if (ProdDto.Image is not null)
                {
                    var root = "/images/Products/";
                    var ProductName = $"{Guid.NewGuid()}" + ProdDto.Image.FileName;
                    if (ProductName.Contains("+"))
                    {
                        ProductName = ProductName.Replace("+", "");
                    }
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }
                    src = root + ProductName;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;

                    using (var fileStream = new FileStream(rootPath, FileMode.Create))
                    {

                        await ProdDto.Image.CopyToAsync(fileStream);
                    }
                }

                if (!string.IsNullOrEmpty(OldProduct.ProductPicture))
                {
                    var picInformation = _fileProvider.GetFileInfo(OldProduct.ProductPicture);
                    var picPath = picInformation.PhysicalPath;
                    System.IO.File.Delete(picPath);


                }

                OldProduct.Name = ProdDto.Name;
                OldProduct.Description = ProdDto.Description;
                OldProduct.Price = ProdDto.Price;
                OldProduct.CategoryId = ProdDto.CategoryId;
                OldProduct.ProductPicture = src;

                _context.Products.Update(OldProduct);
                await _context.SaveChangesAsync();
                    return true;
                }
                return false;


           
        }
    }
}
