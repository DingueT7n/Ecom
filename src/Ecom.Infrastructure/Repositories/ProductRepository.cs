using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration config;

        public ProductRepository(ApplicationDbContext context,IFileProvider fileProvider, IConfiguration config) : base(context)
        {
          _context = context;
            _fileProvider = fileProvider;
            this.config = config;
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

        public async Task<IEnumerable<ProductDtos>> GetAllAsync(ProductParams ProdParam)
        {
            var query = _context.Products
                        .Include(x => x.Category)
                        .AsNoTracking()
                        .AsQueryable();
            if (ProdParam.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == ProdParam.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(ProdParam.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(ProdParam.Search.ToLower()));
            }

            if (!string.IsNullOrEmpty(ProdParam.Sort))
            {
                query = ProdParam.Sort switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Price),
                    "PriceDesc" => query.OrderByDescending(x => x.Price),
                    _ => query.OrderBy(x => x.Name),
                };
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }
            query = query.Skip(ProdParam.PageSiz * (ProdParam.Pagenumber - 1))
                .Take(ProdParam.PageSiz);

            var products = await query.ToListAsync();


            ////performance issue
            //var products = ProdParam.CategoryId.HasValue ? await _context.Products
            //    .Include(x => x.Category)
            //    .Where(x => (x.CategoryId == ProdParam.CategoryId.Value) || (!string.IsNullOrEmpty(ProdParam.Search) ? (x.Name.ToLower() == ProdParam.Search) : false))
            //    .OrderBy(x => x.Name)
            //    .AsNoTracking()
            //    .ToListAsync()
            //    :
            //    await _context.Products
            //    .Include(x => x.Category)
            //    .Where(x => !string.IsNullOrEmpty(ProdParam.Search) ? (x.Name.ToLower() == ProdParam.Search) : true)
            //    .OrderBy(x => x.Name)
            //    .AsNoTracking()
            //    .ToListAsync();


            ////if (ProdParam.CategoryId.HasValue)
            ////    products = products.Where(x=>x.CategoryId == ProdParam.CategoryId.Value).ToList();
            //if (!string.IsNullOrEmpty(ProdParam.Sort))
            //{
            //    products = ProdParam.Sort switch
            //    {
            //        "PriceAsync" => products = (products.OrderBy(x => x.Price)).Skip((ProdParam.PageSiz) * (ProdParam.Pagenumber - 1)).Take(ProdParam.PageSiz).ToList(),
            //        "PriceDesc" => products = (products.OrderByDescending(x => x.Price)).Skip((ProdParam.PageSiz) * (ProdParam.Pagenumber - 1)).Take(ProdParam.PageSiz).ToList(),
            //        _ => products = products.Skip((ProdParam.PageSiz) * (ProdParam.Pagenumber - 1)).Take(ProdParam.PageSiz).ToList(),
            //    };
            //}
            //else
            //    products = products.Skip((ProdParam.PageSiz) * (ProdParam.Pagenumber - 1)).Take(ProdParam.PageSiz).ToList();


            ////products = products.Skip((ProdParam.PageSiz) * (ProdParam.Pagenumber - 1)).Take(ProdParam.PageSiz).ToList();

            var _result = products.Select(x => new ProductDtos
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Price = x.Price,
                CategoryName = x.Category.Name,

                ProductPicture = string.IsNullOrEmpty(x.ProductPicture) ? null : config["ApiURL"] + x.ProductPicture,


            }).ToList();
            return _result;
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
